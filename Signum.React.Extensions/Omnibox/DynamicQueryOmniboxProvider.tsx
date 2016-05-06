﻿import * as React from 'react'
import { OmniboxMessage } from './Signum.Entities.Omnibox'
import { OmniboxResult, OmniboxMatch, OmniboxProvider } from './OmniboxClient'
import { QueryToken, FilterOperation, FindOptions, FilterOption } from '../../../Framework/Signum.React/Scripts/FindOptions'
import * as Navigator from '../../../Framework/Signum.React/Scripts/Navigator'
import * as Finder from '../../../Framework/Signum.React/Scripts/Finder'


const UNKNOWN = "??UNKNOWN??";

export default class DynamicQueryOmniboxProvider extends OmniboxProvider<DynamicQueryOmniboxResult>
{
    getProviderName() {
        return "DynamicQueryOmniboxResult";
    }

    icon() {
        return this.coloredGlyphicon("glyphicon-search", "orange");
    }


    renderItem(result: DynamicQueryOmniboxResult): React.ReactChild[] {

        const array: React.ReactChild[] = [];

        array.push(this.icon());

        this.renderMatch(result.QueryNameMatch, array);

        result.Filters.forEach(f => {
            array.push(<span> </span>);

            var last: string = null;
            if (f.QueryTokenMatches)
                f.QueryTokenMatches.map(m => {
                    if (last != null)
                        array.push(<span>.</span>);
                    this.renderMatch(m, array);
                    last = m.Text;
                });

            if (f.QueryToken.niceName != last) {
                if (last != null)
                    array.push(<span>.</span>);

                array.push(this.coloredSpan(f.QueryTokenOmniboxPascal.tryAfterLast(".") || f.QueryTokenOmniboxPascal, "gray"));
            }

            if (f.CanFilter && f.CanFilter.length)
                array.push(this.coloredSpan(f.CanFilter, "red"));
            else if (f.Operation != null) {

                array.push(<strong>{f.OperationToString}</strong>);

                if (f.Value == UNKNOWN)
                    array.push(this.coloredSpan(OmniboxMessage.Unknown.niceToString(), "red"));
                else if (f.ValueMatch != null)
                    this.renderMatch(f.ValueMatch, array);
                else if (f.Syntax != null && f.Syntax.Completion == FilterSyntaxCompletion.Complete)
                    array.push(<b>{f.ValueToString}</b>);
                else
                    array.push(this.coloredSpan(f.ValueToString, "gray"));
            }   
        });

        return array;
    }

    navigateTo(result: DynamicQueryOmniboxResult) {

        const fo: FindOptions = {
            queryName: result.QueryName,
            filterOptions :[]
        };

        result.Filters.forEach(f => {
            fo.filterOptions.push({
                columnName: f.QueryToken.fullKey,
                token: f.QueryToken,
                operation: f.Operation,
                value: f.Value,
            });
        });

        if (fo.filterOptions.length)
            fo.searchOnLoad = true;

        return Promise.resolve(Finder.findOptionsPath(fo));
    }

    toString(result: DynamicQueryOmniboxResult) {
        var queryName = result.QueryNameMatch.Text;

        var filters = result.Filters.map(f => {

            var token = f.QueryTokenOmniboxPascal;

            if (f.Syntax == null || f.Syntax.Completion == FilterSyntaxCompletion.Token || f.CanFilter && f.CanFilter.length > 1)
                return token;

            var oper = f.OperationToString;

            if (f.Syntax.Completion == FilterSyntaxCompletion.Operation && f.Value == null ||
                (f.Value == UNKNOWN))
                return token + oper;

            return token + oper + f.ValueToString;
        }).join(" ");

        return filters.length ? queryName + " " + filters : queryName;
    }
}

interface DynamicQueryOmniboxResult extends OmniboxResult {
    QueryName: string;
    QueryNameMatch: OmniboxMatch;
    Filters: OmniboxFilterResult[];
}

interface OmniboxFilterResult {

    Distance: number;
    Syntax: FilterSyntax;
    QueryToken: QueryToken;
    QueryTokenOmniboxPascal: string;
    QueryTokenMatches: OmniboxMatch[]
    Operation: FilterOperation;
    OperationToString: string;
    Value: any
    ValueMatch: OmniboxMatch;
    ValueToString: string;
    CanFilter: string;
}

class FilterSyntax {
    Index: number;
    TokenLength: number;
    Length: number;
    Completion: FilterSyntaxCompletion;
}

enum FilterSyntaxCompletion {
    Token = "Token" as any,
    Operation = "Operation" as any,
    Complete = "Complete" as any,
}
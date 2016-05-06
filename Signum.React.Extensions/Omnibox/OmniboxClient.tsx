﻿
import * as React from 'react'
import { Route } from 'react-router'

import { Dic } from '../../../Framework/Signum.React/Scripts/Globals';
import { ajaxPost, ajaxGet } from '../../../Framework/Signum.React/Scripts/Services';
import { EntitySettings } from '../../../Framework/Signum.React/Scripts/Navigator'
import * as Navigator from '../../../Framework/Signum.React/Scripts/Navigator'
import { EntityOperationSettings } from '../../../Framework/Signum.React/Scripts/Operations'
import * as Operations from '../../../Framework/Signum.React/Scripts/Operations'
import { OmniboxMessage } from './Signum.Entities.Omnibox'


export function start(...params: OmniboxProvider<OmniboxResult>[]) {
    params.forEach(op => registerProvider(op));
}

export var providers: { [resultTypeName: string]: OmniboxProvider<OmniboxResult> } = {}; 
export function registerProvider(prov: OmniboxProvider<OmniboxResult>) {
    if (providers[prov.getProviderName()])
        throw new Error(`Provider '${prov.getProviderName()}' already registered`);

    providers[prov.getProviderName()] = prov;
}



export var specialActions: { [resultTypeName: string]: SpecialOmniboxAction } = {};
export function registerSpecialAction(action: SpecialOmniboxAction) {
    if (specialActions[action.key])
        throw new Error(`Action '${action.key}' already registered`);

    specialActions[action.key] = action;
}

export interface SpecialOmniboxAction {
    key: string;
    allowed: () => boolean;
    onClick: () => Promise<string>;
}


export interface HelpOmniboxResult extends OmniboxResult {
    Text: string;
    ReferencedTypeName: string;
}


export function renderItem(result: OmniboxResult): React.ReactChild {
    var items = result.ResultTypeName == "HelpOmniboxResult" ?
        renderHelpItem(result as HelpOmniboxResult) :
        getProvider(result.ResultTypeName).renderItem(result);
    return React.createElement("span", null, ...items);
}

function renderHelpItem(help: HelpOmniboxResult): React.ReactNode[] {

    var result: React.ReactNode[] = [];

    if (help.ReferencedTypeName)
        result.push(getProvider(help.ReferencedTypeName).icon());

    var str = help.Text
        .replaceAll("(", "<strong>")
        .replaceAll(")", "</strong>");

    result.push(<span style={{ fontStyle: "italic" }} dangerouslySetInnerHTML={{ __html: str }}/>);

    return result;
}

export function navigateTo(result: OmniboxResult): Promise<string> {

    if (result.ResultTypeName == "HelpOmniboxResult")
        return Promise.resolve(null);

    return getProvider(result.ResultTypeName).navigateTo(result);
}

export function toString(result: OmniboxResult): string {
    return getProvider(result.ResultTypeName).toString(result);
}

function getProvider(resultTypeName: string) {
    var prov = providers[resultTypeName];

    if (!prov)
        throw new Error(`No provider for '${resultTypeName}'`);

    return prov;
}

export namespace API {

    export function getResults(query: string): Promise<OmniboxResult[]> {
        return ajaxPost<OmniboxResult[]>({ url: "/api/omnibox" }, {
            query: query || "help",
            specialActions: Dic.getKeys(specialActions)
        })
    }
}

export abstract class OmniboxProvider<T extends OmniboxResult> {
    abstract getProviderName(): string;
    abstract renderItem(result: T): React.ReactNode[];
    abstract navigateTo(result: T): Promise<string>;
    abstract toString(result: T): string;
    abstract icon(): React.ReactNode;
    
    renderMatch(match: OmniboxMatch, array: React.ReactNode[]) {

        var regex = /#+/g;

        var last = 0;
        var m: RegExpExecArray;
        while (m = regex.exec(match.BoldMask)) {
            if (m.index > last)
                array.push(<span>{match.Text.substr(last, m.index - last) }</span>);

            array.push(<strong>{match.Text.substr(m.index, m[0].length) }</strong>)

            last = m.index + m[0].length;
        }

        if (last < match.Text.length)
            array.push(<span>{match.Text.substr(last) }</span>);
    }

    coloredSpan(text: string, colorName: string): React.ReactChild {
        return <span style={{ color: colorName, lineHeight: "1.6em" }}>{text}</span>;
    }

    coloredGlyphicon(icon: string, colorName: string): React.ReactChild {
        return <span className={"glyphicon " + icon} style={{ color: colorName, }}/>;
    }
}


export interface OmniboxResult {
    ResultTypeName?: string;
    Distance?: number;
}

export interface OmniboxMatch {
    Distance: number;
    Text: string;
    BoldMask: string;
}


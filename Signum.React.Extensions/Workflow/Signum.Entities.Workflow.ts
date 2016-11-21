//////////////////////////////////
//Auto-generated. Do NOT modify!//
//////////////////////////////////

import { MessageKey, QueryKey, Type, EnumType, registerSymbol } from '../../../Framework/Signum.React/Scripts/Reflection'
import * as Entities from '../../../Framework/Signum.React/Scripts/Signum.Entities'
import * as Basics from '../../../Framework/Signum.React/Scripts/Signum.Entities.Basics'
import * as Authorization from '../Authorization/Signum.Entities.Authorization'
import * as Dynamic from '../Dynamic/Signum.Entities.Dynamic'


interface IWorkflowEvaluator {}


export interface WorkflowEntitiesDictionary {
    [bpmnElementId: string]: Entities.ModelEntity
}

export const BpmnEntityPair = new Type<BpmnEntityPair>("BpmnEntityPair");
export interface BpmnEntityPair extends Entities.EmbeddedEntity {
    Type: "BpmnEntityPair";
    model: Entities.ModelEntity;
    bpmnElementId: string;
}

export const CaseActivityEntity = new Type<CaseActivityEntity>("CaseActivity");
export interface CaseActivityEntity extends Entities.Entity {
    Type: "CaseActivity";
    case: CaseEntity;
    workflowActivity: WorkflowActivityEntity | null;
    originalWorkflowActivityName: string;
    startDate: string;
    doneDate: string | null;
    doneBy: Entities.Lite<Authorization.UserEntity> | null;
}

export module CaseActivityMessage {
    export const OnlyFor0Activites = new MessageKey("CaseActivityMessage", "OnlyFor0Activites");
    export const AlreadyDone = new MessageKey("CaseActivityMessage", "AlreadyDone");
    export const ActivityAlreadyRegistered = new MessageKey("CaseActivityMessage", "ActivityAlreadyRegistered");
    export const CaseContainsOtherActivities = new MessageKey("CaseActivityMessage", "CaseContainsOtherActivities");
    export const NoNextConnectionThatSatisfiesTheConditionsFound = new MessageKey("CaseActivityMessage", "NoNextConnectionThatSatisfiesTheConditionsFound");
}

export module CaseActivityOperation {
    export const Create : Entities.ConstructSymbol_From<CaseActivityEntity, WorkflowEntity> = registerSymbol("Operation", "CaseActivityOperation.Create");
    export const Register : Entities.ExecuteSymbol<CaseActivityEntity> = registerSymbol("Operation", "CaseActivityOperation.Register");
    export const Delete : Entities.DeleteSymbol<CaseActivityEntity> = registerSymbol("Operation", "CaseActivityOperation.Delete");
    export const Next : Entities.ExecuteSymbol<CaseActivityEntity> = registerSymbol("Operation", "CaseActivityOperation.Next");
    export const Approve : Entities.ExecuteSymbol<CaseActivityEntity> = registerSymbol("Operation", "CaseActivityOperation.Approve");
    export const Decline : Entities.ExecuteSymbol<CaseActivityEntity> = registerSymbol("Operation", "CaseActivityOperation.Decline");
}

export module CaseActivityQuery {
    export const Inbox = new QueryKey("CaseActivityQuery", "Inbox");
}

export const CaseEntity = new Type<CaseEntity>("Case");
export interface CaseEntity extends Entities.Entity {
    Type: "Case";
    workflow: WorkflowEntity;
    description: string;
    mainEntity: ICaseMainEntity;
    startDate: string;
    finishDate: string | null;
}

export const CaseJunctionEntity = new Type<CaseJunctionEntity>("CaseJunction");
export interface CaseJunctionEntity extends Entities.Entity {
    Type: "CaseJunction";
    direction?: WorkflowGatewayDirection;
    from?: Entities.Lite<CaseActivityEntity> | null;
    to?: Entities.Lite<CaseActivityEntity> | null;
}

export const CaseNotificationEntity = new Type<CaseNotificationEntity>("CaseNotification");
export interface CaseNotificationEntity extends Entities.Entity {
    Type: "CaseNotification";
    caseActivity?: Entities.Lite<CaseActivityEntity> | null;
    user?: Entities.Lite<Authorization.UserEntity> | null;
    state?: CaseNotificationState;
}

export const CaseNotificationState = new EnumType<CaseNotificationState>("CaseNotificationState");
export type CaseNotificationState =
    "New" |
    "Opened" |
    "InProgress" |
    "Done";

export const DateFilterRange = new EnumType<DateFilterRange>("DateFilterRange");
export type DateFilterRange =
    "All" |
    "LastWeek" |
    "LastMonth" |
    "CurrentYear";

export const DecisionResult = new EnumType<DecisionResult>("DecisionResult");
export type DecisionResult =
    "Approve" |
    "Decline";

export interface ICaseMainEntity extends Entities.Entity {
}

export const InboxFilterModel = new Type<InboxFilterModel>("InboxFilterModel");
export interface InboxFilterModel extends Entities.ModelEntity {
    Type: "InboxFilterModel";
    range?: DateFilterRange;
    states: Entities.MList<CaseNotificationState>;
    fromDate?: string | null;
    toDate?: string | null;
}

export module InboxFilterModelMessage {
    export const Clear = new MessageKey("InboxFilterModelMessage", "Clear");
}

export interface IWorkflowConnectionEntity extends IWorkflowObjectEntity, Entities.Entity {
}

export interface IWorkflowNodeEntity extends IWorkflowObjectEntity, Entities.Entity {
    lane?: WorkflowLaneEntity | null;
}

export interface IWorkflowObjectEntity extends Entities.Entity {
    xml?: WorkflowXmlEntity | null;
    name?: string | null;
}

export const WorkflowActivityEntity = new Type<WorkflowActivityEntity>("WorkflowActivity");
export interface WorkflowActivityEntity extends Entities.Entity, IWorkflowNodeEntity, IWorkflowObjectEntity {
    Type: "WorkflowActivity";
    lane?: WorkflowLaneEntity | null;
    thread?: number;
    name?: string | null;
    description?: string | null;
    type?: WorkflowActivityType;
    viewName?: string | null;
    validationRules: Entities.MList<WorkflowActivityValidationEntity>;
    xml?: WorkflowXmlEntity | null;
}

export module WorkflowActivityMessage {
    export const DuplicateViewNameFound0 = new MessageKey("WorkflowActivityMessage", "DuplicateViewNameFound0");
}

export const WorkflowActivityModel = new Type<WorkflowActivityModel>("WorkflowActivityModel");
export interface WorkflowActivityModel extends Entities.ModelEntity {
    Type: "WorkflowActivityModel";
    mainEntityType: Basics.TypeEntity;
    name?: string | null;
    type?: WorkflowActivityType;
    validationRules: Entities.MList<WorkflowActivityValidationEntity>;
    viewName?: string | null;
    description?: string | null;
}

export module WorkflowActivityOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowActivityEntity> = registerSymbol("Operation", "WorkflowActivityOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowActivityEntity> = registerSymbol("Operation", "WorkflowActivityOperation.Delete");
}

export const WorkflowActivityType = new EnumType<WorkflowActivityType>("WorkflowActivityType");
export type WorkflowActivityType =
    "Task" |
    "DecisionTask";

export const WorkflowActivityValidationEntity = new Type<WorkflowActivityValidationEntity>("WorkflowActivityValidationEntity");
export interface WorkflowActivityValidationEntity extends Entities.EmbeddedEntity {
    Type: "WorkflowActivityValidationEntity";
    rule?: Entities.Lite<Dynamic.DynamicValidationEntity> | null;
    onAccept?: boolean;
    onDecline?: boolean;
}

export const WorkflowConditionEntity = new Type<WorkflowConditionEntity>("WorkflowCondition");
export interface WorkflowConditionEntity extends Entities.Entity {
    Type: "WorkflowCondition";
    name?: string | null;
    mainEntityType?: Basics.TypeEntity | null;
    eval?: WorkflowConnectionEval | null;
}

export module WorkflowConditionOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowConditionEntity> = registerSymbol("Operation", "WorkflowConditionOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowConditionEntity> = registerSymbol("Operation", "WorkflowConditionOperation.Delete");
}

export const WorkflowConnectionEntity = new Type<WorkflowConnectionEntity>("WorkflowConnection");
export interface WorkflowConnectionEntity extends Entities.Entity, IWorkflowConnectionEntity, IWorkflowObjectEntity {
    Type: "WorkflowConnection";
    from?: IWorkflowNodeEntity | null;
    to?: IWorkflowNodeEntity | null;
    name?: string | null;
    decisonResult?: DecisionResult | null;
    condition?: Entities.Lite<WorkflowConditionEntity> | null;
    order?: number;
    xml?: WorkflowXmlEntity | null;
}

export const WorkflowConnectionEval = new Type<WorkflowConnectionEval>("WorkflowConnectionEval");
export interface WorkflowConnectionEval extends Dynamic.EvalEntity<IWorkflowEvaluator> {
    Type: "WorkflowConnectionEval";
}

export const WorkflowConnectionModel = new Type<WorkflowConnectionModel>("WorkflowConnectionModel");
export interface WorkflowConnectionModel extends Entities.ModelEntity {
    Type: "WorkflowConnectionModel";
    name?: string | null;
    decisonResult?: DecisionResult | null;
    condition?: Entities.Lite<WorkflowConditionEntity> | null;
    order?: number;
}

export module WorkflowConnectionOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowConnectionEntity> = registerSymbol("Operation", "WorkflowConnectionOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowConnectionEntity> = registerSymbol("Operation", "WorkflowConnectionOperation.Delete");
}

export const WorkflowEntity = new Type<WorkflowEntity>("Workflow");
export interface WorkflowEntity extends Entities.Entity {
    Type: "Workflow";
    name?: string | null;
    mainEntityType?: Basics.TypeEntity | null;
}

export const WorkflowEventEntity = new Type<WorkflowEventEntity>("WorkflowEvent");
export interface WorkflowEventEntity extends Entities.Entity, IWorkflowNodeEntity, IWorkflowObjectEntity {
    Type: "WorkflowEvent";
    name?: string | null;
    thread?: number;
    lane?: WorkflowLaneEntity | null;
    type?: WorkflowEventType;
    xml?: WorkflowXmlEntity | null;
}

export const WorkflowEventModel = new Type<WorkflowEventModel>("WorkflowEventModel");
export interface WorkflowEventModel extends Entities.ModelEntity {
    Type: "WorkflowEventModel";
    name?: string | null;
    type?: WorkflowEventType;
}

export module WorkflowEventOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowEventEntity> = registerSymbol("Operation", "WorkflowEventOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowEventEntity> = registerSymbol("Operation", "WorkflowEventOperation.Delete");
}

export const WorkflowEventType = new EnumType<WorkflowEventType>("WorkflowEventType");
export type WorkflowEventType =
    "Start" |
    "Finish";

export const WorkflowGatewayDirection = new EnumType<WorkflowGatewayDirection>("WorkflowGatewayDirection");
export type WorkflowGatewayDirection =
    "Split" |
    "Join";

export const WorkflowGatewayEntity = new Type<WorkflowGatewayEntity>("WorkflowGateway");
export interface WorkflowGatewayEntity extends Entities.Entity, IWorkflowNodeEntity, IWorkflowObjectEntity {
    Type: "WorkflowGateway";
    lane?: WorkflowLaneEntity | null;
    thread?: number;
    name?: string | null;
    type?: WorkflowGatewayType;
    direction?: WorkflowGatewayDirection;
    xml?: WorkflowXmlEntity | null;
}

export const WorkflowGatewayModel = new Type<WorkflowGatewayModel>("WorkflowGatewayModel");
export interface WorkflowGatewayModel extends Entities.ModelEntity {
    Type: "WorkflowGatewayModel";
    name?: string | null;
    type?: WorkflowGatewayType;
    direction?: WorkflowGatewayDirection;
}

export module WorkflowGatewayOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowGatewayEntity> = registerSymbol("Operation", "WorkflowGatewayOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowGatewayEntity> = registerSymbol("Operation", "WorkflowGatewayOperation.Delete");
}

export const WorkflowGatewayType = new EnumType<WorkflowGatewayType>("WorkflowGatewayType");
export type WorkflowGatewayType =
    "Exclusive" |
    "Inclusive" |
    "Parallel";

export const WorkflowLaneEntity = new Type<WorkflowLaneEntity>("WorkflowLane");
export interface WorkflowLaneEntity extends Entities.Entity, IWorkflowObjectEntity {
    Type: "WorkflowLane";
    name?: string | null;
    xml?: WorkflowXmlEntity | null;
    pool?: WorkflowPoolEntity | null;
    userOrRoles: Entities.MList<Entities.Lite<Entities.Entity>>;
}

export const WorkflowLaneModel = new Type<WorkflowLaneModel>("WorkflowLaneModel");
export interface WorkflowLaneModel extends Entities.ModelEntity {
    Type: "WorkflowLaneModel";
    name?: string | null;
    userOrRoles: Entities.MList<Entities.Lite<Entities.Entity>>;
}

export module WorkflowLaneOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowLaneEntity> = registerSymbol("Operation", "WorkflowLaneOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowLaneEntity> = registerSymbol("Operation", "WorkflowLaneOperation.Delete");
}

export module WorkflowMessage {
    export const _0BelongsToADifferentWorkflow = new MessageKey("WorkflowMessage", "_0BelongsToADifferentWorkflow");
    export const Condition0IsDefinedFor1Not2 = new MessageKey("WorkflowMessage", "Condition0IsDefinedFor1Not2");
}

export const WorkflowModel = new Type<WorkflowModel>("WorkflowModel");
export interface WorkflowModel extends Entities.ModelEntity {
    Type: "WorkflowModel";
    diagramXml: string;
    entities: Entities.MList<BpmnEntityPair>;
}

export module WorkflowOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowEntity> = registerSymbol("Operation", "WorkflowOperation.Save");
}

export const WorkflowPoolEntity = new Type<WorkflowPoolEntity>("WorkflowPool");
export interface WorkflowPoolEntity extends Entities.Entity, IWorkflowObjectEntity {
    Type: "WorkflowPool";
    workflow?: WorkflowEntity | null;
    name?: string | null;
    xml?: WorkflowXmlEntity | null;
}

export const WorkflowPoolModel = new Type<WorkflowPoolModel>("WorkflowPoolModel");
export interface WorkflowPoolModel extends Entities.ModelEntity {
    Type: "WorkflowPoolModel";
    name?: string | null;
}

export module WorkflowPoolOperation {
    export const Save : Entities.ExecuteSymbol<WorkflowPoolEntity> = registerSymbol("Operation", "WorkflowPoolOperation.Save");
    export const Delete : Entities.DeleteSymbol<WorkflowPoolEntity> = registerSymbol("Operation", "WorkflowPoolOperation.Delete");
}

export const WorkflowReplacementItemEntity = new Type<WorkflowReplacementItemEntity>("WorkflowReplacementItemEntity");
export interface WorkflowReplacementItemEntity extends Entities.EmbeddedEntity {
    Type: "WorkflowReplacementItemEntity";
    oldTask: Entities.Lite<WorkflowActivityEntity>;
    newTask?: string | null;
}

export const WorkflowReplacementModel = new Type<WorkflowReplacementModel>("WorkflowReplacementModel");
export interface WorkflowReplacementModel extends Entities.ModelEntity {
    Type: "WorkflowReplacementModel";
    replacements: Entities.MList<WorkflowReplacementItemEntity>;
}

export const WorkflowXmlEntity = new Type<WorkflowXmlEntity>("WorkflowXmlEntity");
export interface WorkflowXmlEntity extends Entities.EmbeddedEntity {
    Type: "WorkflowXmlEntity";
    diagramXml?: string | null;
}


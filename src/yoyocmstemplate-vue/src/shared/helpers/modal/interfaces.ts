export interface IModalTemplateOptions {
    props: IModalOptions;
    component: any;
    params: any;
}

/**
 * 模态框的参数
 */
export interface IModalOptions {

    prefixCls?: string;
    /** 对话框是否可见*/
    visible?: boolean,
    /** 确定按钮 loading*/
    confirmLoading?: boolean,
    /** 标题*/
    title?: any,
    /** 是否显示右上角的关闭按钮*/
    closable?: boolean,
    /** 点击确定回调*/
    // onOk: (e: React.MouseEvent<any>) => void,
    /** 点击模态框右上角叉、取消按钮、Props.maskClosable 值为 true 时的遮罩层或键盘按下 Esc 时的回调*/
    // onCancel: (e: React.MouseEvent<any>) => void,
    afterClose?: Function,
    /** 垂直居中 */
    centered?: boolean,
    /** 宽度*/
    width?: string | number,
    icon?: any,
    /** 点击蒙层是否允许关闭*/
    maskClosable?: boolean,
    /** 强制渲染 Modal*/
    forceRender?: boolean,
    destroyOnClose?: boolean,
    wrapClassName?: string,
    maskTransitionName?: string,
    transitionName?: string,
    getContainer?: Function,
    zIndex?: number,
    bodyStyle?: Object,
    maskStyle?: Object,
    mask?: boolean,
    keyboard?: boolean,
    wrapProps?: Object,

    // ==================

    close?: any;
    style?: any;
    type?: any;
    class?: any;
}

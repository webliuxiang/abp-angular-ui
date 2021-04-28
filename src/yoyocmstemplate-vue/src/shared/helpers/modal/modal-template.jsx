import Vue from 'vue';

import classNames from 'classnames';
import Dialog from 'ant-design-vue/lib/modal/Modal';
import warning from 'ant-design-vue/lib/_util/warning';


export default {
    functional: true,
    render(h, inputContext) {
        const context = inputContext.props;
        const {props, params} = context;
        const {
            close,
            zIndex,
            afterClose,
            visible,
            keyboard,
            centered,
            getContainer,
            maskStyle,
            closable
        } = props;

        warning(
            !('iconType' in props),
            `The property 'iconType' is deprecated. Use the property 'icon' instead.`,
        );

        const prefixCls = props.prefixCls || 'ant-modal';
        // 默认为 true，保持向下兼容
        const width = props.width || 416;
        const style = props.style || {};
        const mask = props.mask === undefined ? true : props.mask;
        // 默认为 false，保持旧版默认行为
        const maskClosable = props.maskClosable === undefined ? false : props.maskClosable;

        const transitionName = props.transitionName || 'zoom';
        const maskTransitionName = props.maskTransitionName || 'fade';

        const classString = classNames(
            `${props.type}`,
            `${prefixCls}-${props.type}`,
            props.class,
        );


        const DynamicModal = Vue.component(`modal-${(new Date()).valueOf()}`, function (resolve, reject) {
            resolve(context.component);
        });

        const modalRef = {
            close: close
        };

        return (
            <Dialog
                prefixCls={prefixCls}
                class={classString}
                wrapClassName={classNames({[`${prefixCls}-centered`]: !!centered})}
                onCancel={e => close(false)}
                visible={visible}
                closable={closable}
                title=""
                transitionName={transitionName}
                footer=""
                maskTransitionName={maskTransitionName}
                mask={mask}
                maskClosable={maskClosable}
                maskStyle={maskStyle}
                style={style}
                width={width}
                zIndex={zIndex}
                afterClose={afterClose}
                keyboard={keyboard}
                centered={centered}
                getContainer={getContainer}
            >

                {<DynamicModal modelRef={modalRef} modelData={params}/>}

            </Dialog>
        );
    },
};

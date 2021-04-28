import Base from 'ant-design-vue/lib/base';
import {destroyFns} from 'ant-design-vue/lib/modal/Modal';
import {BehaviorSubject, Observable} from 'rxjs';
import {filter} from 'rxjs/operators';
import Vue from 'vue';
import {IModalOptions, IModalTemplateOptions} from './interfaces';
import ModalTempalte from './modal-template';



class ModalHelper {

    // : Promise<boolean>

    /**
     * 动态创建一个组件
     * @param component 组件本身
     * @param params 给组件的参数
     * @param options 弹框的配置
     */
    public create(component: any, params?: any, options?: IModalOptions): Observable<any> {


        const closeSubject = new BehaviorSubject<any>(undefined);


        const div = document.createElement('div');
        const el = document.createElement('div');
        div.appendChild(el);
        document.body.appendChild(div);

        // 基本配置

        options = options ? options : {};
        params = params ? params : {};

        let currentConfig: IModalTemplateOptions = {
            component,
            params,
            props: options,
        };
        currentConfig.props.visible = true;
        currentConfig.props.closable = true;
        currentConfig.props.maskClosable = true;
        currentConfig.props.width = 900;

        if (!options.close) {
            options.close = close;
        } else {
            const customClose = options.close;
            options.close = (args: any) => {
                close(args);
                customClose(args);
            };
        }

        let confirmDialogInstance = null;
        const confirmDialogProps = {props: {}};

        /**
         * 关闭
         * @param args
         */
        function close(args: any) {
            destroy(args);
        }

        /**
         * 更新配置
         * @param newConfig
         */
        function update(newConfig) {
            currentConfig = {
                ...currentConfig,
                ...newConfig,
            };
            confirmDialogProps.props = currentConfig;
        }

        /**
         * 关闭模态框释放资源
         * @param args
         */
        function destroy(args: any) {
            if (confirmDialogInstance && div.parentNode) {
                confirmDialogInstance.$destroy();
                confirmDialogInstance = null;
                div.parentNode.removeChild(div);
            }

            for (let i = 0; i < destroyFns.length; i++) {
                const fn = destroyFns[i];
                if (fn === close) {
                    destroyFns.splice(i, 1);
                    break;
                }
            }

            closeSubject.next(args);
        }

        /**
         * 渲染函数
         * @param config
         */
        function render(config) {
            confirmDialogProps.props = config;

            const V = Base.Vue || Vue;

            return new V({
                el,
                data() {
                    return {
                        confirmDialogProps
                    };
                },
                render() {
                    // 先解构，避免报错，原因不详
                    const cdProps = {...this.confirmDialogProps};
                    return <ModalTempalte {...cdProps} />;
                },
            });
        }

        confirmDialogInstance = render(currentConfig);
        destroyFns.push(close);

        return closeSubject.asObservable().pipe(filter((o) => {
            if (typeof (o) === 'undefined') {
                return false;
            }
            return true;
        }));
    }

}


const modalHelper = new ModalHelper();
export default modalHelper;

import AppCompoentBase from './app-component-base';


const ModalComponentBase = {
    mixins: [AppCompoentBase],
    props: ["modelRef", "modelData"],
    computed: {},
    methods: {
        success(result) {
            if (typeof (result) !== 'boolean') {
                this.modelRef.close(result);
                return;
            }
            this.modelRef.close(true);
        },
        close() {
            this.modelRef.close(false);
        },
        fullData() {
            if (this.modelData) {
                for (let key  in this.modelData) {
                    this[key] = this.modelData[key];
                }
            }
        }
    },
};

export default ModalComponentBase;


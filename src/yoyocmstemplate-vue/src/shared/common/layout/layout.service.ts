import {ILayout, layoutStore_NS, rootStore} from '@/shared/store';

class LayoutService {

    constructor() {
        window.addEventListener('resize', () => {
                this.data.isPad = window.innerWidth < 768;
            },
            false
        );

        this.data.isPad = window.innerWidth < 768;
    }

    get data(): ILayout {
        return rootStore.getters[`${layoutStore_NS}/get`];
    }

    public set(val: ILayout) {
        rootStore.commit(`${layoutStore_NS}/set`, val);
    }

    public changeTheme() {
        this.data.theme = this.data.theme === 'dark' ? 'light' : 'dark';
    }

}

const layoutService = new LayoutService();
export default layoutService;

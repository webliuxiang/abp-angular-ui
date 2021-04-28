/**
 * 应用使用的常量
 */
export class AppConsts {
    /**
     * 远程服务器地址
     */
    public static remoteServiceBaseUrl: string;

    /**
     * 门户地址
     */
    public static portalBaseUrl: string;

    /**
     * 当前应用地址
     */
    public static appBaseUrl: string;

    /**
     * 上传文件api路径
     */
    public static uploadApiUrl = '/api/File/Upload';

    /**
     * 个人头像上传最大MB
     */
    public static maxProfilPictureMb = 1;

    /** 多租户请求头名称 */
    public static tenantIdCookieName = 'Tenant';
    
    /**
     * 后端本地化和前端angular本地化映射
     */
    public static localeMappings: any;

    /**
     * 后端本地化和ng-zorro本地化映射
     */
    public static ngZorroLocaleMappings: any;

    /**
     * 后端本地化和ng-alian本地化映射
     */
    public static ngAlainLocaleMappings: any;

    /**
     * 后端本地化和moment.js本地化映射
     */
    public static momentLocaleMappings: any;

    public static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    public static readonly localization = {
        defaultLocalizationSourceName: 'YoyoCmsTemplate'
    };

    public static readonly authorization = {
        encrptedAuthTokenName: 'enc_auth_token'
    };

    /**
     * 数据表格设置
     */
        // tslint:disable-next-line:member-ordering
    public static readonly grid = {
        /**
         * 每页显示条目数
         */
        defaultPageSize: 10,
        /**
         * 每页显示条目数下拉框值
         */
        defaultPageSizes: [5, 10, 15, 20, 25, 30, 50, 80, 100]
    };

    /**
     * top bar通知组件中获取通知数量
     */
    public static readonly notificationCount = 5;
}

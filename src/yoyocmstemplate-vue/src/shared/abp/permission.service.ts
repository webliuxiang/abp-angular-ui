import {abpService} from '@/shared/abp';
import {IAclService} from '@/shared/common';


class PermissionService implements IAclService {
    public can(acl: string): boolean {
        return abpService.abp.auth.isGranted(acl);
    }

}

const permissionService = new PermissionService();
export default permissionService;

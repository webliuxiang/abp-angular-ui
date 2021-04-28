import { AppConsts } from '@/abpPro/AppConsts';
import { FileDto } from '@/shared/service-proxies/service-proxies';

class FileDownloadService {
    /**
     * 下载临时的导出文件，目前服务于导出excel功能
     * @param file 文件的信息
     */
    public downloadTempFile(file: FileDto) {
        const url = `${AppConsts.remoteServiceBaseUrl}/File/DownloadTempFile?fileType=${file.fileType}
    &fileToken=${file.fileToken}&fileName=${file.fileName}`;
        location.href = url; // TODO: 这将导致在Firefox中重新加载相同的页面,需要等待修复
    }

    /**
     * 下载模板中的文件
     * @param filename 文件名称包含后缀如demo.xlsx
     */
    public downloadTemplateFile(filename: string) {
        const url = `${AppConsts.remoteServiceBaseUrl}/yoyosoft/SampleFiles/${filename}`;
        location.href = url; // TODO: 这将导致在Firefox中重新加载相同的页面,需要等待修复
    }
}

const fileDownloadService = new FileDownloadService();
export default fileDownloadService;

/**
 * 加载第三方脚本的service
 */
class ScriptLoaderService {

    private scripts: any = {};

    public load(...scripts: string[]) {
        this.scripts = scripts;
        const promises: any[] = [];
        scripts.forEach((script) => promises.push(this.loadScript(script)));
        return Promise.all(promises);
    }

    public loadScript(name: string) {
        return new Promise((resolve, reject) => {
            const script = (document.createElement('script') as any);
            script.type = 'text/javascript';
            script.src = name;

            if (script.readyState) {  // IE
                script.onreadystatechange = () => {
                    if (script.readyState === 'loaded' || script.readyState === 'complete') {
                        script.onreadystatechange = null;
                        resolve({script: name, loaded: true, status: 'Loaded'});
                    }
                };
            } else {  // Others
                script.onload = () => {
                    resolve({script: name, loaded: true, status: 'Loaded'});
                };
            }

            script.onerror = (error: any) => resolve({script: name, loaded: false, status: 'Loaded'});
            document.getElementsByTagName('head')[0].appendChild(script);
        });
    }

}

const scriptLoaderService = new ScriptLoaderService();

export {
    scriptLoaderService
};

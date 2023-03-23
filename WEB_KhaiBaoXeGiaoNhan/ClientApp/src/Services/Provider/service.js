import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'providers';//tên controller, thay đổi theo từng controllẻ khác nhau

// Lấy danh sách nhà cung cấp
export function GetProviderList() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+'/find',true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

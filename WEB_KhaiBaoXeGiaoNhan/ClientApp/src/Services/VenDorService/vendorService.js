import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'vendor';//tên controller, thay đổi theo từng controllẻ khác nhau

// Lấy lấy tên  nhà cung cấp
export function GetVendorList(searchText) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

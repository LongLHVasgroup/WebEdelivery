import * as ApiCaller from '../libs/httpRequests';

const prefix = 'Items';//tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetAllItems() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix + '/GetListItem').then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
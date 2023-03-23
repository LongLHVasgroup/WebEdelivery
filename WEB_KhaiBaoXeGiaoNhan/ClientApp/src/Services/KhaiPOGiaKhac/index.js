import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'orderprice';

// lấy danh sách các po đẫ được đánh dấu là giá khác
export function GetListPOGiaKhac() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Xasc nhận PO giá khác
export function ApprovePOGiaKhac(poNumber) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix + `/create?ponumber=${poNumber}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Xoá khai po giá khác
// https://localhost:5001/api/OrderPrice/delete?ponumber=900000
export function HuyPOGiaKhac(poNumber) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix +`/delete?ponumber=${poNumber}`, true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
import * as ApiCaller from '../../Libs/httpRequests';

const prefix = 'driver';//tên controller, thay đổi theo từng controllẻ khác nhau

//trả về json object giống như bên postman gọi ra
export function GetDriverList() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tìm tài xế theo tên hoặc số cmnnd/...
export function SearchDriver(searchText) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet(prefix+"/find?criteria="+searchText,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Cập nhật thông tin tài xế
export function UpdateDriver(body) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPut(prefix+"/capnhatthongtin",body,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Tạo mới
export function AddDriver(body) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost(prefix+"/dangki",body,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};

// Xoá tài xế
export function RemoveDriver(driverId) {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpDelete(prefix+"/delete?ids="+driverId,true).then(respone => {
            return resolve(respone)
        }).catch(err => {
            return reject(err)
        })
    });
};
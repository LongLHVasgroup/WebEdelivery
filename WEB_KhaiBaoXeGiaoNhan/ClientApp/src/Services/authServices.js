
import * as ApiCaller from '../Libs/httpRequests';
import { GetUserInFo } from './AccountSevice/AccountService';
import history from '../history';
// import { store } from '../stores';


export function login(username, password) {

    return new Promise((resolve, reject) => {
        return ApiCaller.httpPost('auth/signin', {
            userName: username,
            password: password,
        }, false)
            .then(objRespone => {
                if (objRespone.isSuccess === true) {
                    // let token = '';
                    // let name = '';
                    // let uid = '';
                    // let role = '';
                    // let type = '';
                    // let username = '';
                    // let isService = false;
                    // if (objRespone.token) {
                    //     token = objRespone.token
                    //     name = objRespone.name || ''
                    //     uid = objRespone.userID || ''
                    //     role = objRespone.role || ''
                    //     type = objRespone.type || ''
                    //     username = objRespone.username || ''
                    //     isService = objRespone.isService || false
                    // }
                    // localStorage.setItem('token', token);
                    // localStorage.setItem('name', name);
                    // localStorage.setItem('uid', uid);
                    // localStorage.setItem('role', role);
                    // localStorage.setItem('type', type);
                    // localStorage.setItem('username', username);
                    // localStorage.setItem('isService', isService);
                    // store.dispatch({ type: 'USER_SET_DATA', data: { token: objRespone.token, name: name, role: objRespone.userID } });
                } else {
                    // store.dispatch({
                    //     type: 'MODAL_OPEN_ERROR_MODAL', errHeader: "Đăng nhập thất bại",
                    //     errContent: objRespone.err.msgString
                    // });

                }
                return resolve(objRespone)

            }).catch(err => {
                // store.dispatch({
                //     type: 'MODAL_OPEN_ERROR_MODAL', errHeader: "Đăng nhập thất bại",
                //     errContent: JSON.stringify(err)
                // });
                return reject(err)
            });
    });

}

export function logout() {
    localStorage.setItem('token', '');
    localStorage.setItem('name', '');
    localStorage.setItem('uid', '');
    localStorage.setItem('role', '');
    localStorage.setItem('type', '');
    localStorage.setItem('username', '');
    localStorage.setItem('company', '');
    localStorage.setItem('email', '');
    localStorage.setItem('phone', '');
    localStorage.setItem('isService', '');
    localStorage.clear();
}
export function checkAuth() {

    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet('Auth/check', true)
            .then(objRespone => {
                return resolve(objRespone)
            }).catch(err => {
                console.log(err)
                // store.dispatch({ type: 'USER_SET_DATA', data: { token: '', name: '', role: '' } });
                if (err === 401 || err === 405) {
                    // sai token
                    logout()
                    history.push('/login');
                }

                return reject(err)
            });
    });
}

export function  checkUserAuth() {
    return new Promise((resolve, reject) => {
        return ApiCaller.httpGet('Auth/check', true)
            .then(objRespone => {
                return resolve(objRespone)
            }).catch(err => {
                return reject(err)
            });
    });
}

export function myCheckAuth() {

    checkAuth()
    let token = localStorage.getItem('token') || '';
    let username = localStorage.getItem('username') || '';
    if (token === '') {
        history.push('/login');
    } else if (username !== '') {
        GetUserInFo(username).then(objRespone => {
            if (objRespone.isSuccess === true) {
                localStorage.setItem('name', objRespone.item.fullName);
                localStorage.setItem('company', objRespone.item.company);
                localStorage.setItem('email', objRespone.item.email);
                localStorage.setItem('phone', objRespone.item.phone);
                localStorage.setItem('type', objRespone.item.type);

            } else {
            }
        }).catch(err => {
        });
    }
}
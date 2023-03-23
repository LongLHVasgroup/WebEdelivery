import React, { useContext, useEffect, useState } from "react";
import { message } from "antd";
// import { GetUserInFo, UpdateUserInfo, ChangePassword } from '../../Services/AccountSevice/AccountService';
import {
  GetUserInFo,
  UpdateUserInfo,
  ChangePassword,
} from "../../../Services/AccountSevice/AccountService";
import ChangePasswordModal from "../../../components/Account/ChangePasswordModal";
import UpdateUserInfoModal from "../../../components/Account/UpdateUserInfoModal";
import HeaderPage from "../../../components/Common/HeaderPage";
import AuthContext from "../../../store/auth-context";
import UserInfo from "../../../components/Account/UserInfo";
import CompanyInfo from "../../../components/Account/CompanyInfo";
import classes from "./index.less";

function Account() {
  const authCtx = useContext(AuthContext);
  const [isLoading, setIsLoading] = useState(true);
  const [userDetails, setUserDetails] = useState({});
  const [isShowUpdateInfo, setIsShowUpdateInfo] = useState(false);
  const [isShowChangePass, setIsShowChangePass] = useState(false);

  const getInfoUser = () => {
    // Call API
    GetUserInFo(authCtx.username).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        setIsLoading(false);
        setUserDetails({
          username: authCtx.username,
          fullName: objRespone.item.fullName,
          email: objRespone.item.email,
          phone: objRespone.item.phone,
          cty: objRespone.item.company,
          address: objRespone.item.address,
          taxCode: objRespone.item.taxcode,
          userType:
            objRespone.item.type === "Customer"
              ? "Khách hàng"
              : objRespone.item.isService === true
              ? "Dịch vụ vận chuyển"
              : objRespone.item.type === "Corrdinator"
              ? "Điều phối viên"
              : objRespone.item.type === "Master"
              ? "Quản lý"
              : "Nhà cung cấp",
        });
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  useEffect(() => {
    getInfoUser();
  }, []);

  // Cập xử lý cập nhật thông tin user
  const updateUserinfo = (values) => {
    message.loading("Đang cập nhật...");
    UpdateUserInfo(values).then((objRespone) => {
      message.destroy();
      if (objRespone.isSuccess === true) {
        getInfoUser();
        message.success(objRespone.err.msgString);
        setIsShowUpdateInfo(false);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  // Bấm nút to show modal form update userinfo
  const handleUpdateInfo = () => {
    setIsShowUpdateInfo(true);
  };

  const checkSameNewPass = (newPass, repeatNewPass) => {
    if (newPass === repeatNewPass) return true;
    return false;
  };

  const handleCloseModal = () => {
    setIsShowUpdateInfo(false);
    setIsShowChangePass(false);
  };

  // Bấm nút to show modal change password
  const handleChangePassword = () => {
    console.log("change password");
    setIsShowChangePass(true);
  };

  // Submit change Password
  const onChangePassword = (values) => {
    if (!checkSameNewPass(values.newPass, values.repeatNewPass)) {
      alert("Mật khẩu mới không trùng nhau");
      return false;
    }
    message.loading("Đang tải...");
    var body = {
      username: authCtx.username,
      oldPassword: values.currentPass,
      newPassword: values.newPass,
    };

    // Call API change password
    ChangePassword(body).then((objRespone) => {
      message.destroy();
      if (objRespone.isSuccess === true) {
        message.success(objRespone.err.msgString);
        setIsShowChangePass(false);
      } else {
        message.error(objRespone.err.msgString);
      }
    });
  };

  return (
    <React.Fragment>
      <HeaderPage title="Tài khoản" description="" />
      {isLoading ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        <>
          {/* ================== */}
          <UserInfo
            username={userDetails.username}
            fullName={userDetails.fullName}
            phone={userDetails.phone}
            email={userDetails.email}
            userType={userDetails.userType}
            handleChangePassword={handleChangePassword}
            handleUpdateInfo={handleUpdateInfo}
          />
          <br />
          <br />
          <CompanyInfo
            cty={userDetails.cty}
            taxCode={userDetails.taxCode}
            address={userDetails.address}
          />
        </>
      )}
      {/* Modal câp nhật thôgn tin user */}
      <UpdateUserInfoModal
        onCancel={handleCloseModal}
        visible={isShowUpdateInfo}
        initialValues={userDetails}
        onFinish={updateUserinfo}
      />

      {/* Modal thay đổi mật khẩu */}
      <ChangePasswordModal
        visible={isShowChangePass}
        onCancel={handleCloseModal}
        onFinish={onChangePassword}
      />
    </React.Fragment>
  );
}
export default Account;

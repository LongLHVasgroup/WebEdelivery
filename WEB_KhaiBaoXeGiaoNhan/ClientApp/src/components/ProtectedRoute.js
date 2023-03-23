import React, { useContext } from "react";
import { Redirect, Route } from "react-router-dom";
import Account from "../pages/Portal/Account";
import PageNotFound from "../components/Exception/PageNotFound";
import { KeHoachGiaoNhan } from "./KeHoachGiaoNhan/KeHoachGiaoNhan";
import { LichSuGiaoNhan } from "./LichSuGiaoNhan/LichSuGiaoNhan";
import KhaiBaoThongTin from "./KhaiBaoThongTin/NCC";
import { KhaiBaoThongTinDVVC } from "./KhaiBaoThongTin/DVVC";
import { VehicleManager } from "./ManagerVehicle/VehicleManager";
import { Driver } from "./Driver/Driver";
import { DieuPhoiPO } from "./DieuPhoiPO";
import { NhapHang } from "./Report/NhapHang";
import { Summary } from "./Report/Summary";
import { TienDoGiaoHang } from "./Report/TienDoGiaoHang";
import { TinhHinhDieuPhoi } from "./Report/TinhHinhDieuPhoi";
import LichSuDieuPhoi from "./LichSuDieuPhoi";
import AuthContext from "../store/auth-context";
import Login from "../pages/Login";
import TinhHinhGiaoHang from "./Report/TinhHinhGiaoHang";
import { TinhHinhDangKy } from "./Report/TinhHinhDangKy";
import { KhaiBaoTheoTau } from "./KhaiBaoTheoTau";
import LichSuKhaiBaoTheoTau from "./LichSuKhaiBaoTau";
import { UpdateKhaiBaoTheoTau } from "./UpdateKhaiBaoTheoTau";
import KhaiBaoNhanHang from "../pages/KhaiBaoNhanHang";

function ProtectedRoute(props) {
  const authCtx = useContext(AuthContext);
  const isLoggedIn = authCtx.isLoggedIn;
  const userType = authCtx.userType;
  const isService = authCtx.isService;

  console.log(authCtx);
  if (!isLoggedIn) {
    if (props.path !== "/login") {
      return (
        <Route>
          <Redirect to="/login" />
        </Route>
      );
    }
    return <Route exact path="/login" component={Login} />;
  }
  if (props.path === "/account") {
    return <Route exact path={props.path} component={Account} />;
  }
  switch (userType) {
    case "Provider":
      switch (props.path) {
        case "/":
          return (
            <Route>
              <Redirect to="/khai-bao-thong-tin" />
            </Route>
          );
        case "/khai-bao-thong-tin":
          if (isService)
            return (
              <Route exact path={props.path} component={KhaiBaoThongTinDVVC} />
            );

          return <Route exact path={props.path} component={KhaiBaoThongTin} />;
        // case "/khai-bao-theo-tau":
        //   return <Route exact path={props.path} component={KhaiBaoTheoTau} />;
        case "/ke-hoach-giao-nhan":
          return <Route exact path={props.path} component={KeHoachGiaoNhan} />;
        case "/lich-su-giao-nhan":
          return <Route exact path={props.path} component={LichSuGiaoNhan} />;
        case "/vehicle-manager":
          return <Route exact path={props.path} component={VehicleManager} />;
        // case "/lich-su-khai-bao-theo-tau":
        //   return <Route exact path={props.path} component={LichSuKhaiBaoTheoTau} />;
        // case "/cap-nhat-khai-bao-theo-tau":
        //   return <Route exact path={props.path} component={UpdateKhaiBaoTheoTau} />
        case "/driver":
          return <Route exact path={props.path} component={Driver} />;
      }
    case "Customer":
      switch (props.path) {
        case "/":
          return (
            <Route>
              <Redirect to="/khai-bao-thong-tin" />
            </Route>
          );
        case "/khai-bao-thong-tin":
          return <Route exact path={props.path} component={KhaiBaoNhanHang} />;
        case "/ke-hoach-giao-nhan":
          return <Route exact path={props.path} component={KeHoachGiaoNhan} />;
        case "/lich-su-giao-nhan":
          return <Route exact path={props.path} component={LichSuGiaoNhan} />;
        case "/vehicle-manager":
          return <Route exact path={props.path} component={VehicleManager} />;
        case "/driver":
          return <Route exact path={props.path} component={Driver} />;
      }
      break;
    case "Corrdinator":
      switch (props.path) {
        case "/":
          return <Redirect to="/dieu-phoi-po" />;
        case "/dieu-phoi-po":
          return <Route exact path={props.path} component={DieuPhoiPO} />;
        case "/bao-cao/nhap-hang":
          return <Route exact path={props.path} component={NhapHang} />;
        case "/bao-cao/tinh-hinh-giao-hang":
          return <Route exact path={props.path} component={TinhHinhGiaoHang} />;
        case "/bao-cao/tinh-hinh-dieu-phoi":
          return <Route exact path={props.path} component={TinhHinhDieuPhoi} />;
        case "/lich-su-dieu-phoi":
          return <Route exact path={props.path} component={LichSuDieuPhoi} />;
        default:
          return <Route exact path={props.path} component={PageNotFound} />;
      }
      break;
    case "Master":
      switch (props.path) {
        case "/":
          return <Redirect to="/summary" />;
        case "/summary":
          return <Route exact path={props.path} component={Summary} />;
        case "/bao-cao/nhap-hang":
          return <Route exact path={props.path} component={NhapHang} />;
        case "/bao-cao/tinh-hinh-giao-hang":
          return <Route exact path={props.path} component={TinhHinhGiaoHang} />;
        case "/bao-cao/tinh-hinh-dieu-phoi":
          return <Route exact path={props.path} component={TinhHinhDieuPhoi} />;
        case "/bao-cao/tinh-hinh-dang-ky-giao-hang":
          return <Route exact path={props.path} component={TinhHinhDangKy} />;
        case "/bao-cao/tien-do-giao-hang":
          return <Route exact path={props.path} component={TienDoGiaoHang} />;
        default:
          return <Route exact path={props.path} component={PageNotFound} />;
      }
      break;
  }
  return <Route exact path={props.path} component={PageNotFound} />;
}

export default ProtectedRoute;

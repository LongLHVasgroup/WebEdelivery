import React from "react";
import { Switch } from "react-router-dom";
import Layout from "./components/Layout";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminLayout from './components/Admin/Layout/AdminLayout'
import "./App.less";
import "./custom.less";

function App() {
  return (
    <Layout>
      <Switch>
        <ProtectedRoute exact path="/" />
        <ProtectedRoute exact path="/login" />
        <ProtectedRoute exact path="/account" />
        <ProtectedRoute exact path="/khai-bao-thong-tin" />
        {/* <ProtectedRoute exact path="/khai-bao-theo-tau" />
        <ProtectedRoute exact path="/lich-su-khai-bao-theo-tau" />
        <ProtectedRoute exact path="/cap-nhat-khai-bao-theo-tau" /> */}

        <ProtectedRoute exact path="/ke-hoach-giao-nhan" />
        <ProtectedRoute exact path="/lich-su-giao-nhan" />
        <ProtectedRoute exact path="/vehicle-manager" />
        <ProtectedRoute exact path="/driver" />
        {/* Cho điều phối */}
        <ProtectedRoute exact path="/dieu-phoi-po" />
        <ProtectedRoute exact path="/lich-su-dieu-phoi" />
        {/* Report */}
        <ProtectedRoute exact path="/bao-cao/nhap-hang" />
        <ProtectedRoute exact path="/summary" />
        <ProtectedRoute exact path="/bao-cao/tinh-hinh-giao-hang" />
        <ProtectedRoute exact path="/bao-cao/tinh-hinh-dieu-phoi" />
        <ProtectedRoute exact path="/bao-cao/tinh-hinh-dang-ky-giao-hang" />
        <ProtectedRoute exact path="/bao-cao/tien-do-giao-hang" />
        
        {/* Customer */}
        
        <ProtectedRoute path="/*" />
      </Switch>
    </Layout>
    // <AdminLayout>

    // </AdminLayout>
  );
}
export default App;

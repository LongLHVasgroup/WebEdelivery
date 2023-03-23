import React, { useState, useContext } from "react";
import { useHistory } from "react-router";
import { Form, Input, Button, Typography } from "antd";
import { login } from "../../Services/authServices";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import AuthContext from "../../store/auth-context";
import "./index.css";

const { Text } = Typography;

function Login() {
  const history = useHistory();
  const authCtx = useContext(AuthContext);

  const [errMessage, setErrMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const onFinish = (values) => {
    setIsLoading(true);
    login(values.username, values.password).then((objRespone) => {
      if (objRespone.isSuccess === true) {
        // if (this.props.location && this.props.location.state) {
        //     const { from } = this.props.location.state;
        //     if (from !== '' && from !== undefined) {
        //         history.push(from);
        //     } else {
        //         history.push('/');
        //     }
        // } else {
        //     history.push('/');
        // }
        const expirationTime = new Date(new Date().getTime() + +3600 * 1000);
        authCtx.login(
          objRespone.token,
          objRespone.type,
          objRespone.username,
          objRespone.isService,
          expirationTime.toISOString()
        );
        history.replace("/");
      } else {
        setIsLoading(false);
        setErrMessage(objRespone.err.msgString);
      }
    });
  };

  return (
    <div className="background">
      <div
        style={{ paddingTop: "4%", paddingBottom: "4%", alignSelf: "center" }}
      >
        <img src="img/ic_logo.svg" alt="Logo" style={{ height: "80px" }} />
      </div>
      <div className="login-form">
        <Form name="normal_login" onFinish={onFinish}>
          <Form.Item className="text-center">
            <h6> ĐĂNG NHẬP</h6>
            <Text type="danger">{errMessage}</Text>
          </Form.Item>
          <Form.Item
            name="username"
            rules={[{ required: true, message: "Nhập tên tài khoản!" }]}
          >
            <Input
              prefix={<UserOutlined className="site-form-item-icon" />}
              placeholder="Tài khoản"
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[
              {
                required: true,
                message: "Nhập mật khẩu!",
              },
            ]}
          >
            <Input.Password
              placeholder="Mật khẩu"
              prefix={<LockOutlined className="site-form-item-icon" />}
            />
          </Form.Item>

          <Form.Item>
            <Button
              className="login-button"
              type="primary"
              htmlType="submit"
              loading={isLoading}
            >
              Đăng Nhập
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
}

export default Login;
// export class Login extends Component {
//     state = {
//         errLogin: "",
//         loading: false
//     };
//     onFinish = values => {
//         this.setState({
//             loading: true
//         })
//         login(values.username, values.password).then((objRespone) => {
//             if (objRespone.isSuccess === true) {
//                 if (this.props.location && this.props.location.state) {
//                     const { from } = this.props.location.state;
//                     if (from !== '' && from !== undefined) {
//                         history.push(from);
//                     } else {
//                         history.push('/');
//                     }
//                 } else {
//                     history.push('/');
//                 }
//             } else {
//                 // message.error(objRespone.err.msgString)
//                 this.setState({
//                     errLogin: objRespone.err.msgString,
//                     loading: false
//                 })
//             }
//         })

//     }

//     render() {
//         return (
//             <div>

//             </div>
//         );
//     }

// }

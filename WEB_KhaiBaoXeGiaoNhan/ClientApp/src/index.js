import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { ConfigProvider } from 'antd';
import vi_VN from 'antd/es/locale/vi_VN';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { AuthContextProvider } from './store/auth-context';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <AuthContextProvider>
    <BrowserRouter basename={baseUrl}>
      <ConfigProvider locale={vi_VN}>
        <App />
      </ConfigProvider>
    </BrowserRouter>
  </AuthContextProvider>,
  rootElement);

registerServiceWorker();


import React from 'react';
import { Button, Result } from 'antd';
import { useHistory } from 'react-router';

function PageNotFound() {
    const history = useHistory();
    return (
        <Result
            status="404"
            title="404"
            subTitle="Không tìm thấy trang."
            extra={
                <Button type="primary" onClick={() => history.push('/')}>Quay về trang chủ</Button>
            }
        />
    );
}
export default PageNotFound;



import { Table, Typography } from 'antd'
import React from 'react'
import TextNumber from '../Common/TextNumber';
import TextPercent from '../Common/TextPercent';
const { Title } = Typography;


const TableLichSuGiaoNhan = (props) => {

    const columns = [
        {
            title: 'Ngày giao nhận',
            dataIndex: 'thoiGianToiDuKien',
            key: 'thoiGianToiDuKien',
            render: (text, row) => <>{row.key === 'total' ? <Title level={5}>{text}</Title> : row.thoiGianToiDuKien.format('DD/MM/YYYY')}</>,
        },
        {
            title: 'Mã đơn hàng',
            dataIndex: 'soDonHang',
            key: 'soDonHang',
        },

        {
            title: 'Số phiếu cân',
            dataIndex: 'scaleTicketCode',
            key: 'scaleTicketCode',
            render: (text) => {
                try {
                    return text.map(item => <p style={{ display: 'inline' }} key={item}>{item}<br /></p>)
                } catch (e) {
                    return text
                }
            }
        },
        {
            title: 'Biển số xe',
            dataIndex: 'vehicleNumber',
            key: 'vehicleNumber',
        },
        {
            title: 'Tài xế',
            dataIndex: 'driverName',
            key: 'driverName',
            render: (text, row) => <p style={{ display: 'inline' }}>{text}<br />{row.driverIdCard}</p>,
        },

        {
            title: 'Thông tin đăng ký',
            children: [
                {
                    title: 'Tên mặt hàng',
                    dataIndex: 'productNameList',
                    key: 'productNameList',
                    render: (text) => (text.map(item => <p style={{ display: 'inline' }} key={item}>{item}<br /></p>)),
                },
                {
                    title: 'Trọng lượng',
                    dataIndex: 'trongLuongGiaoDuKien',
                    key: 'trongLuongGiaoDuKien',
                    render: (text) => <TextNumber value={text} />,
                    align: 'right',
                },
            ],
        },
        {
            title: 'Thông tin thực tế',
            children: [

                {
                    title: 'Tỷ lệ hàng',
                    dataIndex: 'tiLeList',
                    key: 'tiLeList',
                    align: 'right',
                    // width: '3.5%',
                    render: (text) => (text.map(item => <p style={{ display: 'inline' }} >{new Intl.NumberFormat('de-DE').format(item) == 0 ? <>&#8210;</> : new Intl.NumberFormat('de-DE', {
                        style: 'percent', minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    }).format(item)}<br /></p>)),
                },

                {
                    title: 'TL thanh toán',
                    dataIndex: 'trongLuongTTList',
                    key: 'trongLuongTTList',
                    align: 'right',
                    render: (text) => (text.map(item => <p style={{ display: 'inline' }}><TextNumber value={item} /><br /></p>)),
                },
                {
                    title: 'Tạp chất',
                    dataIndex: 'tapChat',
                    key: 'tapChat',
                    align: 'right',
                    render: (text) => <TextNumber value={text} />,
                },
            ]
        },
        {
            title: 'Chênh lệch',
            children: [

                {
                    title: 'Trọng lượng',
                    dataIndex: 'chenhLech',
                    key: 'chenhLech',
                    align: 'right',
                    render: (text) => <TextNumber value={text} />,
                },

                {
                    title: 'Tỷ lệ',
                    dataIndex: 'tiLeChechLech',
                    key: 'tiLeChechLech',
                    align: 'right',
                    render: (text) => <TextPercent value={text} />,
                }
            ]
        }

    ];
    return (
        <Table columns={columns} dataSource={props.dataSource}
            className='my-table-detail-register'
            loading={props.loading}
            pagination={{ showSizeChanger: true }}
            bordered
            locale={{ emptyText: 'Không có dữ liệu' }}
            rowClassName={(record, index) => (record.key == 'total' ? "my-ant-table-cell" : "")}
        />
    )
}

export default TableLichSuGiaoNhan;
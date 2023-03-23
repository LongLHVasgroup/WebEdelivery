using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Models.Common;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Services
{
    public class MergeDataServices : BaseService<MergeDataServices>
    {
        public SingleResponeMessage<MergeDataResult> MergeProvider(string plant)
        {
            MergeDataResult mergeResult = new MergeDataResult();
            SingleResponeMessage<MergeDataResult> ret = new SingleResponeMessage<MergeDataResult>();
            string cnnt = "";
            switch (plant)
            {
                case "3000":
                    cnnt = Config.getInstance().connPMC3000;
                    break;
                case "4000":
                    cnnt = Config.getInstance().connPMC;
                    break;
                case "6000":
                    cnnt = Config.getInstance().connPMC6000;
                    break;
                default:
                    ret.isSuccess = false;
                    ret.item = mergeResult;
                    ret.err.msgCode = "4xx";
                    ret.err.msgString = "Mã plant không đúng";
                    return ret;

            }

            using (SqlConnection connection = new SqlConnection(cnnt))
            {
                var lstProvider = connection.Query<ProviderModel>(@"select * from dbo.ProviderModel").ToList();

                mergeResult = SaveDataProvider(lstProvider);
                mergeResult.Total = lstProvider.Count;
                mergeResult.Source = plant;
                mergeResult.Model = "Provider";
            }


            ret.isSuccess = true;
            ret.item = mergeResult;
            ret.err.msgCode = "2xx";
            ret.err.msgString = "Cập nhật thông tin Provider thành công";
            return ret;
        }

        private MergeDataResult SaveDataProvider(List<ProviderModel> lstProvider)
        {
            MergeDataResult result = new MergeDataResult();
            int update = 0;
            int insert = 0;
            using (SqlConnection connection = new SqlConnection(Config.getInstance().connWeb))
            {
                try
                {
                    foreach (var item in lstProvider)
                    {
                        var queryPars = new DynamicParameters();
                        queryPars.Add("@ProviderCode", item.ProviderCode);
                        var info = connection.Query<ProviderModel>(@"select * from dbo.ProviderModel where ProviderCode = @ProviderCode", queryPars).FirstOrDefault();
                        // var info = ProviderModelDAO.GetInstance().GetList()
                        //                    .Where(s => s.ProviderCode == item.ProviderCode)
                        //                    .FirstOrDefault();

                        if (info != null)
                        {
                            // Thêm mới provider nếu chưa có
                            var updateProvider = new DynamicParameters();
                            updateProvider.Add("@ProviderId", item.ProviderId);
                            updateProvider.Add("@ProviderName", item.ProviderName);
                            updateProvider.Add("@Address", item.Address);
                            updateProvider.Add("@AccountGroup", item.AccountGroup);
                            updateProvider.Add("@isSAPData", item.IsSapdata);
                            updateProvider.Add("@CreatedTime", item.CreatedTime);
                            updateProvider.Add("@LastEditedTime", item.LastEditedTime);
                            updateProvider.Add("@Actived", item.Actived);

                            connection.Execute(@"update ProviderModel set
                                                            ProviderName =@ProviderName ,
                                                            Address =@Address ,
                                                            AccountGroup =@AccountGroup,
                                                            isSAPData=@isSAPData ,
                                                            CreatedTime = @CreatedTime,
                                                            LastEditedTime= @LastEditedTime ,
                                                            Actived = @Actived
                                                    where ProviderId = @ProviderId", updateProvider);
                            update++;

                        }
                        else
                        {
                            // Thêm mới provider nếu chưa có
                            var newProvider = new DynamicParameters();
                            newProvider.Add("@ProviderId", Guid.NewGuid());
                            newProvider.Add("@ProviderCode", item.ProviderCode);
                            newProvider.Add("@ProviderName", item.ProviderName);
                            newProvider.Add("@Address", item.Address);
                            newProvider.Add("@AccountGroup", item.AccountGroup);
                            newProvider.Add("@isSAPData", item.IsSapdata);
                            newProvider.Add("@CreatedTime", item.CreatedTime);
                            newProvider.Add("@LastEditedTime", item.LastEditedTime);
                            newProvider.Add("@Actived", item.Actived);

                            connection.Execute(@" INSERT INTO dbo.ProviderModel
                                                                ( ProviderId,
                                                                ProviderCode ,
                                                                ProviderName ,
                                                                Address ,
                                                                AccountGroup ,
                                                                IsSapdata ,
                                                                CreatedTime,
                                                                LastEditedTime
                                                                )
                                                                values(
                                                                        @ProviderId,
                                                                        @ProviderCode ,
                                                                        @ProviderName,
                                                                        @Address,
                                                                        @AccountGroup ,
                                                                        @IsSapdata ,
                                                                        @CreatedTime ,
                                                                        @LastEditedTime
                                                                        )", newProvider);
                            insert++;

                            // Nếu là NCC mớ trong nước thì thêm vào bảng VehicleOwnerModel 
                            try
                            {
                                if (item.ProviderCode.Substring(0, 1) == "1")
                                {
                                    var newOwner = new DynamicParameters();
                                    newOwner.Add("@VehicleOwner", "V" + item.ProviderCode);
                                    newOwner.Add("@VehicleOwnerName", item.ProviderName);
                                    newOwner.Add("@ProviderCode", item.ProviderCode);
                                    newOwner.Add("@Actived", 1);

                                    connection.Execute(@" INSERT INTO dbo.VehicleOwnerModel
                                                                ( VehicleOwner,
                                                                VehicleOwnerName ,
                                                                ProviderCode ,
                                                                Actived
                                                                )
                                                                values(
                                                                        @VehicleOwner,
                                                                        @VehicleOwnerName ,
                                                                        @ProviderCode,
                                                                        @Actived
                                                                        )", newOwner);
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Updated = update;
                    result.Inserted = insert;
                    return result;
                }
            }
            result.Updated = update;
            result.Inserted = insert;
            return result;
        }
    }
}

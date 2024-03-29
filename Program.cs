﻿using Microsoft.SqlServer.Server;
using SPP.Econtract1._0;
using SPP.Econtract2._0;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity;
using System.IO;

namespace SPP.Migration
{


    public static class ContractConsts
    {
        public struct RoleName
        {
            public const string Applicant = "Applicant";
            public const string Function_Manager_I = "Function Manager I";
            public const string Function_Manager_II = "Function Manager II";
            public const string Purchasing_I = "Purchasing I";
            public const string Purchasing_II = "Purchasing II";
            public const string SCM_I = "SCM I";
            public const string SCM_II = "SCM II";
            public const string Finance_I = "Finance I";
            public const string Finance_II = "Finance II";
            public const string Legal_Approver_I = "Legal Approver I";
            public const string Legal_Approver_II = "Legal Approver II";
            public const string Legal_Customer_I = "Legal Customer I";
            public const string Legal_Customer_II = "Legal Customer II";
            public const string Legal_Service_I = "Legal Service I";
            public const string Legal_Service_II = "Legal Service II";
            public const string Legal_Customer_ABC = "Legal Customer ABC";
            public const string Legal_Customer_NDA = "Legal Customer NDA";
            public const string OPM = "OPM";
            public const string OPD = "OPD";
            public const string OP_Assistant = "OP Assistant";
            public const string Upload_PIC = "Upload PIC";
            public const string Upload_PIC_FM = "Upload PIC FM";
            public const string File_In_PIC = "File In PIC";

        }

        public struct OldRoleName
        {
            public const string Applicant = "Applicant";
            public const string Applicant1 = "Applicant1";
            public const string Applicant2 = "Applicant2";
            public const string FM1 = "FM1";
            public const string FM2 = "FM2";
            public const string PUR = "PUR";
            public const string SCM = "SCM";
            public const string FINANCE = "FINANCE";
            public const string FINANCE2 = "FINANCE2";
            public const string LEGAL = "LEGAL";
            public const string LEGAL2 = "LEGAL2";
            public const string OPM = "OPM";
            public const string OPA = "OPA";
            public const string OPA1 = "OPA1";
            public const string Upload_User = "Upload User";
            public const string DCC = "DCC";

        }

        public struct OldUserRole
        {
            public const string CONTRACT_CREATOR = "CONTRACT_CREATOR";
            public const string CONTRACT_VIEWER = "CONTRACT_VIEWER";
            public const string CONTRACT_ADMIN = "CONTRACT_ADMIN";
            public const string CONTRACT_REVIEWER = "CONTRACT_REVIEWER";
            public const string OP_ASSISTANT = "OP_ASSISTANT";
        }

        public struct NewUserRole
        {
            public const string Contract_Approver = "Contract_Approver";
            public const string Contract_Viewer = "Contract_Viewer";
            public const string Contract_Creator = "Contract_Creator";
            public const string E_Contract_Admin = "E-Contract_Admin";
            public const string E_Contract_OPA = "E-Contract_OPA";
        }

        public struct OldReviewStatus
        {
            public const string FM1_Review = "FM1 Review";
            public const string FM2_Review = "FM2 Review";
            public const string PUR_Review = "PUR Review";
            public const string SCM_Review = "SCM Review";
            public const string Finance_Review = "Finance Review";
            public const string Finance2_Review = "Finance2 Review";
            public const string Legal_Review = "Legal Review";
            public const string OPM_Review = "OPM Review";
            public const string Approved = "Approved";
            public const string OPM_Approved = "OPM Approved";
            public const string Stamping = "Stamping";
            public const string Upload = "Upload";
            public const string DCC_File_In = "DCC File In";
            public const string Completed = "Completed";
            public const string Withdraw = "Withdraw";
            public const string Rejected = "Rejected";
        }

        public struct NewReviewStatus
        {

            public const string Function_Manager_I_Review = "Function Manager I Review";
            public const string Function_Manager_II_Review = "Function Manager II Review";
            public const string Under_Review = "Under Review";
            public const string OPM_Review = "OPM Review";
            public const string OPD_Review = "OPD Review";
            public const string Approved = "Approved";
            public const string To_Stamp = "To Stamp";
            public const string Stamping = "Stamping";
            public const string Upload = "Upload";
            public const string File_In = "File In";
            public const string Completed = "Completed";
            public const string Withdraw = "Withdraw";
            public const string Rejected = "Rejected";
        }

        public struct Task
        {
            public const string Submit = "Submit";
            public const string Review = "Review";
            public const string Recheck = "Recheck";
            public const string ReadyToStamp = "ReadyToStamp";
            public const string Stamping_Done = "Stamping Done";
            public const string Upload = "Upload";
            public const string File_In = "File In";

        }

    }

    class Program
    {

        #region 辅助方法-------------------Add By Hongzhong 2017/05

        public static string GetStatus(string oldStatus)
        {
            var new_status = oldStatus;
            switch (oldStatus)
            {

                case ContractConsts.OldReviewStatus.FM1_Review:
                    new_status = ContractConsts.NewReviewStatus.Function_Manager_I_Review;
                    break;
                case ContractConsts.OldReviewStatus.FM2_Review:
                    new_status = ContractConsts.NewReviewStatus.Function_Manager_II_Review;
                    break;


                case ContractConsts.OldReviewStatus.PUR_Review:
                    new_status = ContractConsts.NewReviewStatus.Under_Review;
                    break;
                case ContractConsts.OldReviewStatus.SCM_Review:
                    new_status = ContractConsts.NewReviewStatus.Under_Review;
                    break;
                case ContractConsts.OldReviewStatus.Finance_Review:
                    new_status = ContractConsts.NewReviewStatus.Under_Review;
                    break;
                case ContractConsts.OldReviewStatus.Finance2_Review:
                    new_status = ContractConsts.NewReviewStatus.Under_Review;
                    break;
                case ContractConsts.OldReviewStatus.Legal_Review:
                    new_status = ContractConsts.NewReviewStatus.Under_Review;
                    break;


                case ContractConsts.OldReviewStatus.OPM_Approved:
                    new_status = ContractConsts.NewReviewStatus.To_Stamp;
                    break;

                case ContractConsts.OldReviewStatus.DCC_File_In:
                    new_status = ContractConsts.NewReviewStatus.File_In;
                    break;

            }

            return new_status;


        }

        public static string GetRoleName(string RoleName)
        {
            var new_roleName = RoleName;
            switch (RoleName)
            {
                case ContractConsts.OldRoleName.Applicant:
                    new_roleName = ContractConsts.RoleName.Applicant;
                    break;
                case ContractConsts.OldRoleName.Applicant1:
                    new_roleName = ContractConsts.RoleName.Applicant;
                    break;
                case ContractConsts.OldRoleName.Applicant2:
                    new_roleName = ContractConsts.RoleName.Applicant;
                    break;
                case ContractConsts.OldRoleName.FM1:
                    new_roleName = ContractConsts.RoleName.Function_Manager_I;
                    break;
                case ContractConsts.OldRoleName.FM2:
                    new_roleName = ContractConsts.RoleName.Function_Manager_II;
                    break;
                case ContractConsts.OldRoleName.PUR:
                    new_roleName = ContractConsts.RoleName.Purchasing_I;
                    break;
                case ContractConsts.OldRoleName.SCM:
                    new_roleName = ContractConsts.RoleName.SCM_I;
                    break;
                case ContractConsts.OldRoleName.FINANCE:
                    new_roleName = ContractConsts.RoleName.Finance_I;
                    break;
                case ContractConsts.OldRoleName.FINANCE2:
                    new_roleName = ContractConsts.RoleName.Finance_II;
                    break;
                case ContractConsts.OldRoleName.LEGAL:
                    new_roleName = ContractConsts.RoleName.Legal_Approver_I;
                    break;
                case ContractConsts.OldRoleName.LEGAL2:
                    new_roleName = ContractConsts.RoleName.Legal_Approver_II;
                    break;
                case ContractConsts.OldRoleName.OPM:
                    new_roleName = ContractConsts.RoleName.OPM;
                    break;
                case ContractConsts.OldRoleName.OPA:
                    new_roleName = ContractConsts.RoleName.OP_Assistant;
                    break;
                case ContractConsts.OldRoleName.OPA1:
                    new_roleName = ContractConsts.RoleName.OP_Assistant;
                    break;
                case ContractConsts.OldRoleName.Upload_User:
                    new_roleName = ContractConsts.RoleName.Upload_PIC;
                    break;
                case ContractConsts.OldRoleName.DCC:
                    new_roleName = ContractConsts.RoleName.File_In_PIC;
                    break;


            }

            return new_roleName;

        }

        private static string GetConnectString()
        {
            string conStr = ConfigurationManager.ConnectionStrings["SPP_MVC_Entities"].ConnectionString;
            int i = conStr.IndexOf("data source");
            conStr = conStr.Substring(i);
            i = conStr.IndexOf('"');
            conStr = conStr.Substring(0, i);
            return conStr;
        }

        public static void BulkInsert<T>(string connection, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BulkCopyTimeout = 600;
                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                                           .Cast<PropertyDescriptor>()
                                           .Where(propertyInfo => propertyInfo.PropertyType != null
                                                && propertyInfo.PropertyType.Namespace != null
                                                && propertyInfo.PropertyType.Namespace.Equals("System"))
                                           .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }

        public static void starMethodName(string name)
        {
            Console.WriteLine(Runfunction(name) + " " + DateTime.Now.ToString() + " 开始运行.");

        }

        public static void endMethodName(string name)
        {
            Console.WriteLine(Runfunction(name) + " " + DateTime.Now.ToString() + " 结束运行.");
        }

        public static string Runfunction(string name)

        {

            string re = name;
            switch (name)
            {

                default:
                    break;
                case
                    "MigrationProduction":
                    re =name+ "正在修正旧架构历史错误资料";
                    break;
                case
                    "Insert_Tb_Users":
                    re = name + "正在创建用户";
                    break;
                case
                    "Insert_Tb_Company":
                    re = name + "正在创建公司";
                    break;
                case
                    "Insert_Tb_DepartMent":
                    re = name + "正在创建部门";
                    break;
                case
                    "Insert_Tb_Users_CompanyDepartment":
                    re = name + "正在关联用户部门";
                    break;
                case
                    "Insert_Tb_ContractType_M":
                    re = name + "正在创建合约类型";
                    break;
                case
                    "Insert_Tb_ContractType_D":
                    re = name + "正在创建合约明细类型";
                    break;
                case
                    "Insert_Tb_ContractTemplate":
                    re = name + "正在创建合约模板";
                    break;
                case
                    "Insert_TypeCode_Data":
                    re = name + "正在创建一级联动表";
                    break;
                case
                    "Insert_Tb_Module":
                    re = name + "正在创建ModuleID";
                    break;
                case
                    "Insert_Contract_M":
                    re = name + "正在创建合约主表";
                    break;
                case
                    "Insert_Tb_Contract_Attachment":
                    re = name + "正在创建合约附件表";
                    break;
                case
                    "Insert_Tb_Contract_WfTeam":
                    re = name + "正在创建合约人员审批配置表";
                    break;
                case
                    "Insert_Tb_WfTaskDelaySetting":
                    re = name + "正在创建用户延期天数设定";
                    break;
                case
                    "Insert_Tb_WfEmail_StopExpirationNotice":
                    re = name + "正在创建用户邮件提醒";
                    break;
                case
                    "Insert_Users_Role":
                    re = name + "正在导入用户角色";
                    break;
                case
                    "Insert_Tb_WfTask":
                    re = name + "正在创建 WFTask";
                    break;
                case
                    "Update_WFTask":
                    re = name + "正在更新 WFTask";
                    break;
                case
                    "Insert_Tb_WfTask_History":
                    re = name + "正创建 Insert_Tb_WfTask_History";
                    break;
            }
            return re;
            //MigrationProduction();//正在修正旧架构历史错误资料
            //Insert_Tb_Users();//创建用户
            //Insert_Tb_Company();
            //Insert_Tb_DepartMent();
            //Insert_Tb_Users_CompanyDepartment();
            //Insert_Tb_ContractType_M();
            //Insert_Tb_ContractType_D();
            //Insert_Tb_ContractTemplate();
            //Contract_Attachment_ToCMD();//生成Contract_Attachment 拷贝指令
            //Insert_TypeCode_Data();
            ////Insert_Tb_Module();//导入ModuleID 信息
            //Insert_Contract_M();

            //////导入申请人权限
            //Insert_Users_Role();
            //Insert_Tb_Contract_Attachment();
            //Update_Tb_Contract_Attachment();
            //Insert_Tb_Contract_WfTeam();
            //Insert_Tb_WfTaskDelaySetting();
            //Insert_Tb_WfDelegation();
            //Insert_Tb_WfDelegation_History();
            //Insert_Tb_WfEmail_StopExpirationNotice();
            //Insert_Tb_WfTask();
            //Update_WFTask();
            //Insert_Tb_WfTask_History();



        }

        public static void Delete_ALL()
        {
            using (var context = new SPP_MVC_Entities())
            {
                var contract_m = context.Contract_M.ToList();
                var users = context.Users.Where(m => m.Modified_Remarks == modify_remarks).ToList();
                var company = context.Company.Where(m => m.Modified_Remarks == modify_remarks).ToList();
                var department = context.Department.Where(m => m.Modified_Remarks == modify_remarks).ToList();
                var users_company_department = context.Users_CompanyDepartment.Where(m => m.Modified_Remarks == modify_remarks).ToList();
                var contracttype_m = context.ContractType_M.ToList();
                var contracttype_d = context.ContractType_D.ToList();
                var contracttemplate = context.ContractTemplate.ToList();


                context.Contract_M.RemoveRange(contract_m);
                context.Users.RemoveRange(users);
                context.Company.RemoveRange(company);
                context.Department.RemoveRange(department);
                context.Users_CompanyDepartment.RemoveRange(users_company_department);
                context.ContractTemplate.RemoveRange(contracttemplate);
                context.ContractType_D.RemoveRange(contracttype_d);
                context.ContractType_M.RemoveRange(contracttype_m);
                context.SaveChanges();
            }
        }

        public static void overMethodName()
        {
            Console.WriteLine("全部执行完毕.");
            Console.ReadKey();
        }


        #endregion


        #region 公用变量-------------------Add By Hongzhong 2017/05
        public static Guid modified_guid = new Guid("0B08C006-5AB5-E611-83F5-005056BF221C");
        public static string modify_remarks = "E-Contract 1.0 data migration to E-Contract 2.0";
        public static Guid module_uid = new Guid("39326F1E-54C6-4ED7-9D2D-A0415F8321D3");
        public static string wf_cancel_remarks = "Canceled by System Admin, due to E-Contract 1.0 data migration to E-Contract 2.0.";

        #endregion

        static void Main(string[] args)
        {
            //Insert_Tb_Contract_Attachment();
            //Insert_Tb_WfTask_History();
            #region Contract Applicant -------------------Add By Hongzhong 2017/04
            //Delete_ALL();
            //Insert_Tb_Users();
            //Insert_Tb_Company();
            //Insert_Tb_DepartMent();
            //Insert_Tb_Users_CompanyDepartment();
            //Insert_Tb_ContractType_M();
            //Insert_Tb_ContractType_D();
            //Insert_Tb_ContractTemplate();
            //Insert_TypeCode_Data();
            //Insert_Tb_Module();
            //Insert_Contract_M();
            //Insert_Tb_Contract_Attachment();
            //Insert_Tb_Contract_WfTeam();
            //Insert_Tb_WfTaskDelaySetting();
            //Insert_Tb_WfDelegation();
            //Insert_Tb_WfDelegation_History();
            //Insert_Tb_WfEmail_StopExpirationNotice();
            //Insert_Tb_WfTask();
            //Insert_Tb_WfTask_History();
            ////Delete_ALL();
            //test();
            #endregion
            //migration();
            //Insert_Users_Role();
            //Insert_Users_Role();
            //test_DataWarranty_End_Date

            //Insert_Users_Role();
            //Insert_Tb_WfTaskDelaySetting();


            //======================

            //Insert_Contract_M();
            //Insert_Tb_Contract_Attachment();
            //Update_Tb_Contract_Attachment();
            //Insert_Tb_WfTask();
            //Update_WFTask();
            //Insert_Tb_WfTask_History();
            //Insert_Tb_Contract_WfTeam();
            //Insert_Tb_WfTaskDelaySetting();
            Insert_Tb_ContractTemplate();
            overMethodName();


        }

        public static void migration()
        {
            MigrationProduction();
            //正在修正旧架构Proudction 历史错误资料
            Insert_Tb_Users();//创建用户
            Insert_Tb_Company();
            Insert_Tb_DepartMent();
            Insert_Tb_Users_CompanyDepartment();
            Insert_Tb_ContractType_M();
            Insert_Tb_ContractType_D();
            Insert_Tb_ContractTemplate();
           
            Insert_TypeCode_Data();
            //Insert_Tb_Module();//导入ModuleID 信息
            Insert_Contract_M();

            ////导入申请人权限
            Insert_Users_Role();
            Insert_Tb_Contract_Attachment();
            Update_Tb_Contract_Attachment();
            Contract_Attachment_ToCMD();//生成Contract_Attachment 拷贝指令
            Insert_Tb_Contract_WfTeam();
            Insert_Tb_WfTaskDelaySetting();
            Insert_Tb_WfDelegation();
            Insert_Tb_WfDelegation_History();
            Insert_Tb_WfEmail_StopExpirationNotice();
            Insert_Tb_WfTask();
            Update_WFTask();
            Insert_Tb_WfTask_History();

        }

        public static void Insert_Tb_Users()
        {
            //SELECT* FROM  dbo.Users WHERE User_NTID = 'LiuK9' 数据重复

            //初始化查询
            List<SYSTEM_USERS> users_spp_list = new List<SYSTEM_USERS>();
            using (var context = new SPP_ProductionEntities())
            {
                //string sql = @"
                //    SELECT *,RN FROM
                //    (
                //    SELECT *, ROW_NUMBER() OVER(PARTITION BY ACCOUNT ORDER BY  GETDATE()) AS RN FROM SYSTEM_USERS  WHERE Dept LIKE '%Contract%' or ( Dept IS NULL)
                //    ) t WHERE t.RN = 1
                //                        ";
                //WHERE Dept LIKE '%Contract%' or(Dept IS NULL)or(Account = 'Lia9')
                string sql = @"
                    SELECT *,RN FROM
                    (
                    SELECT *, ROW_NUMBER() OVER(PARTITION BY ACCOUNT ORDER BY  GETDATE()) AS RN FROM SYSTEM_USERS    WHERE    account<>'yangy9'  and  account<>'YangY225'
                    ) t WHERE t.RN = 1
                                        ";
                //account <> 'LiuK9'  and
                users_spp_list = context.SYSTEM_USERS.SqlQuery(sql).ToList();

            }

            //已存在ntid
            List<string> users_sppmvc_list = new List<string>();
            using (var context = new SPP_MVC_Entities())
            {
                users_sppmvc_list = context.Users.Select(m => m.User_NTID.ToLower()).ToList<string>();
            }

            //去除已存在的ntid
            users_spp_list = users_spp_list.Where(m => !users_sppmvc_list.Contains(m.ACCOUNT.ToLower())).ToList();


            //插入数据.
            List<Users> users_list = new List<Users>();
            if (users_spp_list.Count > 0)
            {
                using (var context = new SPP_MVC_Entities())
                {
                    foreach (var item in users_spp_list)
                    {

                        Users model_Users = new Users();
                        model_Users.Users_UID = Guid.NewGuid();
                        model_Users.User_NTID = item.ACCOUNT;
                        model_Users.User_Name = item.USER_NAME;
                        model_Users.Is_Enable = true;
                        model_Users.Email = item.EMAIL;
                        model_Users.Tel = item.JVN_TEL;
                        model_Users.Login_Token = null;
                        model_Users.Modified_UID = modified_guid;
                        model_Users.Modified_Date = DateTime.Now;
                        model_Users.Modified_Remarks = modify_remarks;
                        users_list.Add(model_Users);
                    }
                    context.Users.AddRange(users_list);
                    context.SaveChanges();
                }

            }


        }

        public static void Insert_Tb_Company()
        {
            List<SYSTEM_PLANT> system_plant_spp_list = new List<SYSTEM_PLANT>();
            using (var context = new SPP_ProductionEntities())
            {

                //var sql = @"
                //          SELECT * FROM
                //          (
                //          SELECT * ,ROW_NUMBER() OVER (PARTITION BY  CCODE ORDER BY  GETDATE() ) AS RN FROM dbo.SYSTEM_PLANT
                //          ) t WHERE t.RN=1
                //          ";

                var sql = @"
    SELECT * FROM
                          (
                          SELECT * ,ROW_NUMBER() OVER (PARTITION BY  CCODE ORDER BY  GETDATE() ) AS RN FROM dbo.SYSTEM_PLANT
                          ) t WHERE t.RN=1 AND  t.NAME_2 LIKE '%E-Contract%'
";

                system_plant_spp_list = context.SYSTEM_PLANT.SqlQuery(sql).ToList();

            }

            //已存在CompanyCode
            List<string> company_sppmvc_list = new List<string>();
            using (var context = new SPP_MVC_Entities())
            {
                company_sppmvc_list = context.Company.Select(m => m.Company_Code).ToList<string>();
            }
            //去除已存在的CompanyCode
            system_plant_spp_list = system_plant_spp_list.Where(m => !company_sppmvc_list.Contains(m.CCODE)).ToList();

            //插入数据
            List<Company> comany_list = new List<Company>();
            using (var context = new SPP_MVC_Entities())
            {
                foreach (var item in system_plant_spp_list)
                {
                    Company model_Company = new Company();
                    model_Company.Company_UID = Guid.NewGuid();
                    model_Company.Company_Code = item.CCODE;
                    model_Company.Company_Name_ZH = item.LEGAL_ENTITY_ZH;
                    model_Company.Company_Name_EN = item.LEGAL_ENTITY_EN;
                    model_Company.Address_ZH = item.ADDRESS_ZH;
                    model_Company.Address_EN = item.ADDRESS_EN;
                    model_Company.Is_Enable = true;
                    model_Company.Modified_UID = modified_guid;
                    model_Company.Modified_Date = DateTime.Now;
                    model_Company.Modified_Remarks = modify_remarks;
                    comany_list.Add(model_Company);
                }
                context.Company.AddRange(comany_list);
                context.SaveChanges();
            }



        }

        public static void Insert_Tb_DepartMent()
        {

            //SITE_CODE    DEPARTMENT Company_Code    Department_UID Company_UID 关系 旧架构
            //            var sql = @"
            //SELECT  DISTINCT a.SITE_CODE,a.DEPARTMENT,b.Company_Code,
            //'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
            //AS Department_UID,
            //'' AS Company_UID 
            //FROM WF_REVIEW_TEAM_CONTRACT_SITE a
            //LEFT JOIN 
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_CODE=b.SITE_CODE GROUP BY  a.SITE_CODE,a.DEPARTMENT,b.Company_Code    ORDER BY  a.SITE_CODE,DEPARTMENT
            //";

            //            var sql = @"
            //SELECT DISTINCT a.SITE_CODE ,a.DEPARTMENT  ,b.Company_Code,
            //'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
            //AS Department_UID,
            //'' AS Company_UID 
            //FROM dbo.SYSTEM_DEPARTMENT a
            //LEFT JOIN  
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_CODE=b.SITE_CODE GROUP BY  a.SITE_CODE,a.DEPARTMENT,b.Company_Code    ORDER BY  a.SITE_CODE,DEPARTMENT
            //";

            //            var sql = @"
            //		SELECT DISTINCT SITE_NAME AS SITE_CODE ,DEPARTMENT AS DEPARTMENT,
            //			b.Company_Code,
            //'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
            //AS Department_UID,
            //'' AS Company_UID 
            //			 FROM 
            //			(
            //			SELECT DISTINCT SITE_NAME,DEPARTMENT FROM  dbo.SYSTEM_USER_DEPARTMENT
            //			UNION ALL
            //			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM dbo.SYSTEM_DEPARTMENT
            //			)a 
            //			LEFT JOIN  
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_NAME=b.SITE_CODE GROUP BY  a.SITE_NAME,a.DEPARTMENT,b.Company_Code    ORDER BY  a.SITE_NAME,DEPARTMENT
            //";

            //            var sql = @"
            //	SELECT DISTINCT SITE_NAME AS SITE_CODE ,DEPARTMENT AS DEPARTMENT,
            //			b.Company_Code,
            //'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
            //AS Department_UID,
            //'' AS Company_UID 
            //			 FROM 
            //			(
            //		     SELECT DISTINCT SITE_NAME,DEPARTMENT FROM  dbo.SYSTEM_USER_DEPARTMENT
            //			UNION ALL
            //			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM dbo.SYSTEM_DEPARTMENT
            //			UNION ALL
            //			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM  dbo.CONTRACT_M
            //			UNION ALL
            //			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM dbo.WF_REVIEW_TEAM_CONTRACT_SITE
            //			)a 
            //			LEFT JOIN  
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_NAME=b.SITE_CODE GROUP BY  a.SITE_NAME,a.DEPARTMENT,b.Company_Code    ORDER BY  a.SITE_NAME,DEPARTMENT
            //";


            //            var sql = @"
            //SELECT t.*,y.COST_CENTER FROM  

            //(
            //SELECT DISTINCT SITE_NAME AS SITE_CODE ,DEPARTMENT AS DEPARTMENT,
            //			b.Company_Code,
            //'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
            //AS Department_UID,
            //'' AS Company_UID 
            //			 FROM 
            //			(

            //		    SELECT DISTINCT SITE_NAME,DEPARTMENT FROM  dbo.SYSTEM_USER_DEPARTMENT
            //			UNION ALL
            //			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM dbo.WF_REVIEW_TEAM_CONTRACT_SITE

            //			)a 
            //			LEFT JOIN  
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_NAME=b.SITE_CODE GROUP BY  a.SITE_NAME,a.DEPARTMENT,b.Company_Code 
            //) t LEFT JOIN  
            //(SELECT DISTINCT SITE_CODE,DEPARTMENT ,COST_CENTER FROM dbo.WF_REVIEW_TEAM_CONTRACT_SITE WHERE Cost_center IS NOT NULL )y 
            //ON  t.SITE_CODE=y.SITE_CODE  AND t.DEPARTMENT=y.DEPARTMENT



            //";

            var sql = @"
                     SELECT DISTINCT SITE_NAME AS SITE_CODE ,DEPARTMENT AS DEPARTMENT,
			b.Company_Code,
'95BCBAB5-544C-465A-A6F1-4B9DBD'+ RIGHT('000000'+CAST(ROW_NUMBER()OVER( ORDER BY GETDATE() ) AS nvarchar(50)),6 )
AS Department_UID,
'' AS Company_UID 
			 FROM 
			(

		    SELECT DISTINCT SITE_NAME,DEPARTMENT FROM  dbo.SYSTEM_USER_DEPARTMENT
			UNION ALL
			SELECT DISTINCT SITE_CODE,DEPARTMENT FROM dbo.WF_REVIEW_TEAM_CONTRACT_SITE

			)a 
			LEFT JOIN  
(
select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
) b ON  a.SITE_NAME=b.SITE_CODE GROUP BY  a.SITE_NAME,a.DEPARTMENT,b.Company_Code        
";

            List<Insert_Tb_DepartMent_1> insert_tb_department_1_list = new List<Insert_Tb_DepartMent_1>();

            List<Company> sppmvc_company_list = new List<Company>();
            using (var context = new SPP_MVC_Entities())
            {
                sppmvc_company_list = context.Company.ToList();
            }


            //Company_UID 赋值
            using (var context = new SPP_ProductionEntities())
            {
                insert_tb_department_1_list = context.Database.SqlQuery<Insert_Tb_DepartMent_1>(sql).ToList();
                foreach (var item in insert_tb_department_1_list)
                {
                    item.Company_UID = sppmvc_company_list.Where(m => m.Company_Code == item.Company_Code).FirstOrDefault().Company_UID.ToString();
                }
            }

            //新增
            using (var context = new SPP_MVC_Entities())
            {
                List<Department> department_list = new List<Department>();
                foreach (var item in insert_tb_department_1_list)
                {
                    Department model_Department = new Department();
                    model_Department.Department_UID = new Guid(item.Department_UID);
                    model_Department.Company_UID = new Guid(item.Company_UID);
                    model_Department.SAP_CostCenter = String.Empty;
                    if (!string.IsNullOrEmpty(item.COST_CENTER))
                    {
                        model_Department.SAP_CostCenter = item.COST_CENTER;
                    }
                    model_Department.Department_Name = item.DEPARTMENT;
                    model_Department.Is_Enable = true;
                    model_Department.Modified_UID = modified_guid;
                    model_Department.Modified_Date = DateTime.Now;
                    model_Department.Modified_Remarks = modify_remarks;
                    department_list.Add(model_Department);
                }
                context.Department.AddRange(department_list);
                context.SaveChanges();
            }



        }

        public static void Insert_Tb_Users_CompanyDepartment()
        {
            //            string sql = @"
            //SELECT  DISTINCT a.SITE_CODE,a.DEPARTMENT,a.SUBMIT,b.Company_Code,
            //''
            //AS Department_UID,
            //'' AS Company_UID ,

            //NEWID() AS Users_CompanyDepartment_UID ,
            //'' AS Users_UID
            //FROM WF_REVIEW_TEAM_CONTRACT_SITE a
            //LEFT JOIN 
            //(
            //select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
            //) b ON  a.SITE_CODE=b.SITE_CODE GROUP BY  a.SITE_CODE,a.DEPARTMENT,b.Company_Code ,a.SUBMIT   ORDER BY  a.SITE_CODE,DEPARTMENT

            //";
            string sql = @"
SELECT  DISTINCT a.SITE_NAME AS SITE_CODE,a.DEPARTMENT,a.ACCOUNT as SUBMIT,b.Company_Code,
''
AS Department_UID,
'' AS Company_UID ,

NEWID() AS Users_CompanyDepartment_UID ,
'' AS Users_UID
FROM SYSTEM_User_DEPARTMENT a
LEFT JOIN 
(
select distinct NAME_0 as SITE_CODE , CCODE  as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE
) b ON  a.SITE_NAME=b.SITE_CODE GROUP BY  a.SITE_NAME,a.DEPARTMENT,b.Company_Code ,a.ACCOUNT   ORDER BY  a.SITE_NAME,DEPARTMENT
";

            List<Department> sppmvc_department_list = new List<Department>();
            List<Company> sppmvc_company_list = new List<Company>();
            List<Users> sppmvc_users_list = new List<Users>();
            using (var context = new SPP_MVC_Entities())
            {
                sppmvc_department_list = context.Department.ToList();
                sppmvc_company_list = context.Company.ToList();
                sppmvc_users_list = context.Users.ToList();

            }

            List<Insert_Tb_Users_CompanyDepartment_1> insert_tb_users_companydepartment = new List<Insert_Tb_Users_CompanyDepartment_1>();
            using (var context = new SPP_ProductionEntities())
            {
                insert_tb_users_companydepartment = context.Database.SqlQuery<Insert_Tb_Users_CompanyDepartment_1>(sql).ToList();
                //赋值Company_UID
                foreach (var item in insert_tb_users_companydepartment)
                {
                    item.Company_UID = sppmvc_company_list.Where(m => m.Company_Code == item.Company_Code).FirstOrDefault().Company_UID.ToString();

                }
                //根据 赋值Company_UID，DepartMentName 赋值 Department_UID  .根据NTID 赋值Users_UID
                foreach (var item in insert_tb_users_companydepartment)
                {

                    var depart = sppmvc_department_list.Where(m => m.Company_UID == new Guid(item.Company_UID) & m.Department_Name == item.DEPARTMENT).FirstOrDefault();
                    if (depart != null)
                    {
                        item.Department_UID = depart.Department_UID.ToString();
                        item.Users_UID = sppmvc_users_list.Where(m => m.User_NTID.ToLower() == item.SUBMIT.ToLower()).FirstOrDefault().Users_UID.ToString();

                    }
                    else
                    {
                        item.Department_UID = null;
                    }

                }
            }
            //过滤不存在 的 insert_tb_users_companydepartment
            insert_tb_users_companydepartment = insert_tb_users_companydepartment.Where(m => m.Department_UID != null).ToList();
            using (var context = new SPP_MVC_Entities())
            {
                //插入 Users_CompanyDepartment
                List<Users_CompanyDepartment> Users_CompanyDepartment_list = new List<Users_CompanyDepartment>();
                foreach (var item in insert_tb_users_companydepartment)
                {
                    Users_CompanyDepartment model_Users_CompanyDepartment = new Users_CompanyDepartment();
                    model_Users_CompanyDepartment.Users_CompanyDepartment_UID = item.Users_CompanyDepartment_UID;
                    model_Users_CompanyDepartment.Users_UID = new Guid(item.Users_UID);
                    model_Users_CompanyDepartment.Company_UID = new Guid(item.Company_UID);
                    model_Users_CompanyDepartment.Department_UID = new Guid(item.Department_UID);
                    model_Users_CompanyDepartment.Begin_Date = DateTime.Now;
                    model_Users_CompanyDepartment.End_Date = null;
                    model_Users_CompanyDepartment.Modified_UID = modified_guid;
                    model_Users_CompanyDepartment.Modified_Date = DateTime.Now;
                    model_Users_CompanyDepartment.Modified_Remarks = modify_remarks;
                    Users_CompanyDepartment_list.Add(model_Users_CompanyDepartment);

                }
                context.Users_CompanyDepartment.AddRange(Users_CompanyDepartment_list);
                context.SaveChanges();

            }

        }

        public static void Insert_Tb_ContractType_M()
        {
            Delete_ContractType();

            List<string> contracttype_m_str = new List<string>();
            List<ContractType_M> contract_type_m_list = new List<ContractType_M>();
            using (var context = new SPP_ProductionEntities())
            {
                var sql = @"SELECT DISTINCT TYPE_CATEGORY FROM CONTRACT_TYPE_MASTER";
                contracttype_m_str = context.Database.SqlQuery<string>(sql).ToList();

                foreach (var item in contracttype_m_str)
                {
                    ContractType_M model_ContractType_M = new ContractType_M();
                    model_ContractType_M.ContractType_M_UID = Guid.NewGuid();
                    model_ContractType_M.ContractType_M_Name = item.ToString();
                    model_ContractType_M.Is_Enable = true;
                    model_ContractType_M.Modified_UID = modified_guid;
                    model_ContractType_M.Modified_Date = DateTime.Now;
                    model_ContractType_M.Modified_Remarks = modify_remarks;
                    contract_type_m_list.Add(model_ContractType_M);
                }

            }
            using (var context = new SPP_MVC_Entities())
            {

                context.ContractType_M.AddRange(contract_type_m_list);
                context.SaveChanges();
            }
        }

        private static string Legal_Customer = "Customer";//特殊1阶合约类型
        private static string[] Legal_Customer_ABC = { "ABC Agreement (Customer)" };//只需要2ND_LEGAL_CUSTOMER
        private static string Legal_Customer_ABC_ONE = "ABC Agreement (Customer)";//只需要LEGAL_CUSTOMER

        private static string[] Legal_Service =
    {
"VMI/HUB (Customer)",
"Customs/Shipping/Carriage/Courier/Logistic Agreement",
"Hub/VMI Agreement (Supplier)"
};//特殊类型合约需要两阶Legal审核

        private static string Legal_Customer_NDA_ONE =  "NDA (Customer)" ;//只需要Legal_Customer_NDA 审核

        public static void Insert_Tb_ContractType_D()
        {

            List<ContractType_M> ContractType_M_list = new List<ContractType_M>();
            List<CONTRACT_TYPE_MASTER> CONTRACT_TYPE_MASTER_list = new List<CONTRACT_TYPE_MASTER>();

            using (var context = new SPP_ProductionEntities())
            {
                CONTRACT_TYPE_MASTER_list = context.CONTRACT_TYPE_MASTER.ToList();

            }
            List<ContractType_D> ContractType_D_list = new List<ContractType_D>();

            using (var context = new SPP_MVC_Entities())
            {
                ContractType_M_list = context.ContractType_M.ToList();
                foreach (var item in CONTRACT_TYPE_MASTER_list)
                {
                    ContractType_D model_ContractType_D = new ContractType_D();
                    model_ContractType_D.ContractType_D_UID = Guid.NewGuid();
                    model_ContractType_D.ContractType_M_UID = ContractType_M_list.Where(m => m.ContractType_M_Name == item.TYPE_CATEGORY).FirstOrDefault().ContractType_M_UID;
                    model_ContractType_D.ContractType_D_Name_EN = item.TYPE_CODE;
                    model_ContractType_D.ContractType_D_Name_ZH = item.TEMPLATE_NOTE;
                    model_ContractType_D.Period_Required = item.PERIOD_REQUIRED.HasValue ? Convert.ToBoolean(item.PERIOD_REQUIRED) : false;
                    model_ContractType_D.Need_Finance = item.ISFINANCEREVIEW.HasValue ? Convert.ToBoolean(item.ISFINANCEREVIEW) : false;
                    model_ContractType_D.Need_Purchasing = item.ISPURREVIEW.HasValue ? Convert.ToBoolean(item.ISPURREVIEW) : false;
                    model_ContractType_D.Need_SupplyChain = item.ISSCMREVIEW.HasValue ? Convert.ToBoolean(item.ISSCMREVIEW) : false;
                    model_ContractType_D.Need_Legal_General = item.ONLYLEGALREVIEW.HasValue ? Convert.ToBoolean(item.ONLYLEGALREVIEW) : false;

                    model_ContractType_D.Need_Legal_CustomerABC = false;
                    model_ContractType_D.Need_Legal_CustomerNDA = false;

                    model_ContractType_D.Need_Legal_Customer = false;
                    model_ContractType_D.Need_Legal_Service = false;

                    if (item.TYPE_CODE.Trim() == Legal_Customer_ABC_ONE)
                    {
                        model_ContractType_D.Need_Legal_CustomerABC = true;
                    }
                    if (item.TYPE_CODE.Trim() == Legal_Customer_NDA_ONE)
                    {
                        model_ContractType_D.Need_Legal_CustomerNDA = true;
                    }

                    if (item.TYPE_CATEGORY.Trim() == Legal_Customer)
                    {
                        model_ContractType_D.Need_Legal_Customer = true;
                    }

                    if (Legal_Service.Contains(item.TYPE_CATEGORY.Trim()) == true)
                    {
                        model_ContractType_D.Need_Legal_Service = true;
                    }

                    model_ContractType_D.Is_Enable = true;
                    model_ContractType_D.Modified_UID = modified_guid;
                    model_ContractType_D.Modified_Date = DateTime.Now;
                    model_ContractType_D.Modified_Remarks = modify_remarks;
                    ContractType_D_list.Add(model_ContractType_D);
                }
                context.ContractType_D.AddRange(ContractType_D_list);
                context.SaveChanges();
            }

        }

        public static void Insert_Tb_ContractTemplate()
        {
            List<Insert_Tb_ContractTemplate_1> insert_tb_contracttemplate_1 = new List<Insert_Tb_ContractTemplate_1>();
            List<Company> company_list = new List<Company>();
            List<ContractType_D> contract_d_list = new List<ContractType_D>();

            List<ContractType_M> contract_type_m_list = new List<ContractType_M>();
            using (var context = new SPP_MVC_Entities())
            {
                company_list = context.Company.ToList();
                contract_type_m_list = context.ContractType_M.ToList();
                contract_d_list = context.ContractType_D.ToList();


            }

            using (var context = new SPP_ProductionEntities())
            {
                //过滤重复部门的表单
                //                var sql = @"
                //SELECT * FROM
                //(
                //SELECT [CONTRACT_TYPE_UID], [TYPE_GROUP], [TYPE_CODE], [TEMPLATE_NAME], [TEMPLATE_PATH], [TEMPLATE_DESC], [VERSION], [DEL_MK], [CREATOR], [CREATE_DATE], [MODIFIER], [MODIFY_DATE], [PLANT_LOCATION], [PLANT_TYPE], [ONLYLEGALREVIEW], [PERIOD_REQUIRED], [TEMPLATE_NOTE], [ISFINANCEREVIEW], [ISPURREVIEW], [ISSCMREVIEW], [TYPE_CATEGORY] 
                //,b.CCODE,'' AS Company_UID,'' AS ContractType_M_UID,ROW_NUMBER() OVER (PARTITION BY  TYPE_CODE,CCODE,TEMPLATE_NAME ORDER BY GETDATE()) AS RN
                // FROM (SELECT * FROM  dbo.CONTRACT_TYPE WHERE LEN(TEMPLATE_NAME)>10 ) a
                //LEFT JOIN dbo.SYSTEM_PLANT b
                //ON a.PLANT_LOCATION=b.LOCATION AND  a.PLANT_TYPE=b.TYPE
                //) t WHERE t.RN=1 AND t.DEL_MK='N'




                //           ";

                var sql = @"
SELECT * FROM
(
SELECT [CONTRACT_TYPE_UID], [TYPE_GROUP], [TYPE_CODE], [TEMPLATE_NAME], [TEMPLATE_PATH], [TEMPLATE_DESC], [VERSION], [DEL_MK], [CREATOR], [CREATE_DATE], [MODIFIER], [MODIFY_DATE], [PLANT_LOCATION], [PLANT_TYPE], [ONLYLEGALREVIEW], [PERIOD_REQUIRED], [TEMPLATE_NOTE], [ISFINANCEREVIEW], [ISPURREVIEW], [ISSCMREVIEW], [TYPE_CATEGORY] 
,b.CCODE,'' AS Company_UID,'' AS ContractType_M_UID,ROW_NUMBER() OVER (PARTITION BY  TYPE_CODE,CCODE,TEMPLATE_DESC ORDER BY GETDATE()) AS RN
 FROM (SELECT * FROM  dbo.CONTRACT_TYPE WHERE LEN(TEMPLATE_NAME)>10 ) a
LEFT JOIN dbo.SYSTEM_PLANT b
ON a.PLANT_LOCATION=b.LOCATION AND  a.PLANT_TYPE=b.TYPE
) t WHERE t.RN=1 AND t.DEL_MK='N'


";

                insert_tb_contracttemplate_1 = context.Database.SqlQuery<Insert_Tb_ContractTemplate_1>(sql).ToList();

                //赋值Company_UID、CONTRACT_TYPE_UID
                foreach (var item in insert_tb_contracttemplate_1)
                {
                    item.Company_UID = company_list.Where(m => m.Company_Code == item.CCODE).FirstOrDefault().Company_UID.ToString();
                    //item.CONTRACT_TYPE_UID = contract_type_m_list.Where(m => m.ContractType_M_Name == item.TYPE_CATEGORY).FirstOrDefault().ContractType_M_UID.ToString();
                    item.CONTRACT_TYPE_UID = contract_d_list.Where(m => m.ContractType_D_Name_EN == item.TYPE_CODE).FirstOrDefault().ContractType_D_UID.ToString();
                }
            }

            using (var context = new SPP_MVC_Entities())
            {
                var users = context.Users.ToList();

                List<ContractTemplate> ContractTemplate_list = new List<ContractTemplate>();
                foreach (var item in insert_tb_contracttemplate_1)
                {

                    ContractTemplate model_ContractTemplate = new ContractTemplate();
                    model_ContractTemplate.ContractTemplate_UID = Guid.NewGuid();
                    model_ContractTemplate.Company_UID = new Guid(item.Company_UID);
                    model_ContractTemplate.ContractType_D_UID = new Guid(item.CONTRACT_TYPE_UID);
                    model_ContractTemplate.System_File_Name = "";
                    model_ContractTemplate.Original_File_Name = item.TEMPLATE_NAME;
                    //model_ContractTemplate.Display_File_Name = item.TEMPLATE_NAME;//-------------

                    model_ContractTemplate.Display_File_Name = item.TEMPLATE_DESC+".doc";//-------------需求2
                    model_ContractTemplate.File_Size = 0;
                    model_ContractTemplate.File_Path = "";
                    model_ContractTemplate.Tempkey = Guid.Empty;

                    if (item.DEL_MK == "Y")
                    {
                        model_ContractTemplate.Is_Enable = false;
                    }
                    else
                    {
                        model_ContractTemplate.Is_Enable = true;
                    }
                    if (!string.IsNullOrEmpty(item.MODIFIER))
                    {
                        model_ContractTemplate.Modified_UID = users.Where(m => m.User_NTID == item.MODIFIER).FirstOrDefault().Users_UID;
                    }
                    else
                    {
                        model_ContractTemplate.Modified_UID = modified_guid;//----
                    }
                   
                    //model_ContractTemplate.Modified_Date = DateTime.Now;
                    model_ContractTemplate.Modified_Date = item.MODIFY_DATE;//-----需求3
                    model_ContractTemplate.Modified_Remarks = item.TEMPLATE_DESC;

                    var newFileName = model_ContractTemplate.Modified_Date.ToString("yyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + ".x";
                    var newFilePath = string.Format(@"Temp/{0}", newFileName);
                    model_ContractTemplate.System_File_Name = newFileName;//导入改名称
                    model_ContractTemplate.File_Path = @"FileVault/" + newFileName;

                    ContractTemplate_list.Add(model_ContractTemplate);
                }
                ContractTemplate_list.Count();

                context.ContractTemplate.AddRange(ContractTemplate_list);
                context.SaveChanges();

            }


        }

        public static void Delete_ContractType()
        {
            using (var context = new SPP_MVC_Entities())
            {
                var contracttype_m = context.ContractType_M.ToList();
                var contracttype_d = context.ContractType_D.ToList();
                var contracttemplate = context.ContractTemplate.ToList();
                var contract_m = context.Contract_M.ToList();
                var contract_d = context.Contract_Attachment.ToList();

                context.Contract_Attachment.RemoveRange(contract_d);
                context.SaveChanges();
                context.Contract_M.RemoveRange(contract_m);
                context.SaveChanges();
                context.ContractTemplate.RemoveRange(contracttemplate);
                context.SaveChanges();
                context.ContractType_D.RemoveRange(contracttype_d);
                context.SaveChanges();
                context.ContractType_M.RemoveRange(contracttype_m);
                context.SaveChanges();
            }



        }

        public static void Insert_TypeCode_Data()
        {
            Guid renew_guid = new Guid("E705C006-5AB5-E611-83F5-005056BF221C");
            TypeCode_L1 model_TypeCode_L1 = new TypeCode_L1();
            model_TypeCode_L1.TypeCode_L1_UID = renew_guid;
            model_TypeCode_L1.TypeCode_L1_ID = "S013";
            model_TypeCode_L1.TypeCode_L1_Name = "Contract IsRenew";
            model_TypeCode_L1.Begin_Date = DateTime.Now;
            model_TypeCode_L1.End_Date = null;
            model_TypeCode_L1.Reserved_1 = null;
            model_TypeCode_L1.Reserved_2 = null;
            model_TypeCode_L1.Remarks = null;
            model_TypeCode_L1.Modified_UID = modified_guid;
            model_TypeCode_L1.Modified_Date = DateTime.Now;
            model_TypeCode_L1.Modified_Remarks = null;

            List<TypeCode_L2> TypeCode_L2_list = new List<TypeCode_L2>();

            {
                TypeCode_L2 model_TypeCode_L2 = new TypeCode_L2();
                model_TypeCode_L2.TypeCode_L2_UID = new Guid("E9AFA82A-BCFA-46BE-8A8A-E13F74B4FF59");
                model_TypeCode_L2.TypeCode_L1_UID = renew_guid;
                model_TypeCode_L2.TypeCode_L2_ID = "S013-01";
                model_TypeCode_L2.TypeCode_L2_Name = "New Contract";
                model_TypeCode_L2.Begin_Date = DateTime.Now;
                model_TypeCode_L2.End_Date = null;
                model_TypeCode_L2.Reserved_1 = null;
                model_TypeCode_L2.Reserved_2 = null;
                model_TypeCode_L2.Remarks = null;
                model_TypeCode_L2.Modified_UID = modified_guid;
                model_TypeCode_L2.Modified_Date = DateTime.Now;
                model_TypeCode_L2.Modified_Remarks = null;
                TypeCode_L2_list.Add(model_TypeCode_L2);
            }

            {
                TypeCode_L2 model_TypeCode_L2 = new TypeCode_L2();
                model_TypeCode_L2.TypeCode_L2_UID = new Guid("13A6D3EE-ED95-4EC2-9E6E-C21E8131A9AB");
                model_TypeCode_L2.TypeCode_L1_UID = renew_guid;
                model_TypeCode_L2.TypeCode_L2_ID = "S013-02";
                model_TypeCode_L2.TypeCode_L2_Name = "Renew with same contract";
                model_TypeCode_L2.Begin_Date = DateTime.Now;
                model_TypeCode_L2.End_Date = null;
                model_TypeCode_L2.Reserved_1 = null;
                model_TypeCode_L2.Reserved_2 = null;
                model_TypeCode_L2.Remarks = null;
                model_TypeCode_L2.Modified_UID = modified_guid;
                model_TypeCode_L2.Modified_Date = DateTime.Now;
                model_TypeCode_L2.Modified_Remarks = null;
                TypeCode_L2_list.Add(model_TypeCode_L2);
            }

            {
                TypeCode_L2 model_TypeCode_L2 = new TypeCode_L2();
                model_TypeCode_L2.TypeCode_L2_UID = new Guid("523D5162-EE87-4E16-9474-0FFCB89242DF");
                model_TypeCode_L2.TypeCode_L1_UID = renew_guid;
                model_TypeCode_L2.TypeCode_L2_ID = "S013-03";
                model_TypeCode_L2.TypeCode_L2_Name = "Renew with differ contract";
                model_TypeCode_L2.Begin_Date = DateTime.Now;
                model_TypeCode_L2.End_Date = null;
                model_TypeCode_L2.Reserved_1 = null;
                model_TypeCode_L2.Reserved_2 = null;
                model_TypeCode_L2.Remarks = null;
                model_TypeCode_L2.Modified_UID = modified_guid;
                model_TypeCode_L2.Modified_Date = DateTime.Now;
                model_TypeCode_L2.Modified_Remarks = null;
                TypeCode_L2_list.Add(model_TypeCode_L2);
            }

            using (var context = new SPP_MVC_Entities())
            {

                var type_code1_model = context.TypeCode_L1.Where(m => m.TypeCode_L1_UID == renew_guid).ToList();
                if (type_code1_model.Count > 0)
                {
                    context.TypeCode_L1.RemoveRange(type_code1_model);
                }

                var type_code2_list = context.TypeCode_L2.Where(m => m.TypeCode_L1_UID == renew_guid).ToList();
                if (type_code2_list.Count > 0)
                {
                    context.TypeCode_L2.RemoveRange(type_code2_list);
                }
                context.SaveChanges();
                context.TypeCode_L1.Add(model_TypeCode_L1);
                context.TypeCode_L2.AddRange(TypeCode_L2_list);
                context.SaveChanges();

            }



        }

        public static void Insert_Tb_Module()
        {

            using (var context = new SPP_MVC_Entities())
            {
                Module model_Module = new Module();
                model_Module.Module_UID = module_uid;
                model_Module.Module_Name = "E-Contract";
                model_Module.Users_PIC_UIDs = "1F08C006-5AB5-E611-83F5-005056BF221C";
                model_Module.System_PIC_UIDs = "3908C006-5AB5-E611-83F5-005056BF221C,4A08C006-5AB5-E611-83F5-005056BF221C,0B08C006-5AB5-E611-83F5-005056BF221C,1608C006-5AB5-E611-83F5-005056BF221C";
                model_Module.Is_Enable = true;
                model_Module.Modified_UID = modified_guid;
                model_Module.Modified_Date = DateTime.Now;
                model_Module.Modified_Remarks = "Users: Amber; System: CM, Amanda, Hongzhong, Eugene";
                context.Module.Add(model_Module);
                context.SaveChanges();
            }

        }

        public static void Insert_Contract_M()
        {
            starMethodName("Insert_Contract_M");
            List<Users> users = new List<Users>();
            List<Company> company = new List<Company>();
            List<Department> department = new List<Department>();
            List<ContractType_D> contract_d = new List<ContractType_D>();
            List<TypeCode_L2> typecode = new List<TypeCode_L2>();
            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.ToList();
                company = context.Company.ToList();
                department = context.Department.ToList();
                contract_d = context.ContractType_D.ToList();
                //typecode = context.TypeCode_L2.ToList();
            }

            //            var sql = @"
            //                SELECT a.* ,b.Company_Code,'' AS Applicant_UID,'' AS Department_UID,'' AS ContractType_D_UID,'' AS Is_Renew_UID,'' as JabilEntity_UID  FROM dbo.CONTRACT_M a LEFT JOIN  (select distinct NAME_0 as SITE_CODE, CCODE as Company_Code  from SYSTEM_PLANT, SYSTEM_USER_PLANT
            //where SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION
            //and SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE) b
            //ON a.SITE_CODE = b.SITE_CODE WHERE a.CONTRACT_TYPE<>'' 
            //AND CONTRACT_NO NOT IN ('09361602031','09361604008','09361604028','09361604030','GPB1512002','HUA-2015-0393-IE')
            //                ";

            //            var sql = @"
            //           SELECT   a.* ,
            //                    b.Company_Code ,
            //                    '' AS Applicant_UID ,
            //                    '' AS Department_UID ,
            //                    '' AS ContractType_D_UID ,
            //                    '' AS Is_Renew_UID ,
            //                    '' AS JabilEntity_UID,
            //                    c.NT_ACCOUNT  AS Modify_NTID
            //           FROM     dbo.CONTRACT_M a
            //           LEFT JOIN ( SELECT DISTINCT
            //                                NAME_0 AS SITE_CODE ,
            //                                CCODE AS Company_Code
            //                       FROM     SYSTEM_PLANT ,
            //                                SYSTEM_USER_PLANT
            //                       WHERE    SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION AND SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE ) b ON a.SITE_CODE = b.SITE_CODE
            //           LEFT JOIN dbo.CHANGE_HISTORY c
            //           ON  a.CONTRACT_M_UID=c.OBJ_UID
            //           WHERE    a.CONTRACT_TYPE <> '' AND CONTRACT_NO NOT IN ( '09361602031', '09361604008', '09361604028', '09361604030', 'GPB1512002', 'HUA-2015-0393-IE' )  AND  c.NT_ACCOUNT IS NOT NULL

            //";
            var sql = @"
            SELECT a.* ,
                    b.Company_Code ,
                    '' AS Applicant_UID,
                    '' AS Department_UID,
                    '' AS ContractType_D_UID,
                    '' AS Is_Renew_UID,
                    '' AS JabilEntity_UID,
                    c.NT_ACCOUNT AS Modify_NTID,
                       c.MODIFY_DATETIME AS MODIFY_DATETIME
          FROM     dbo.CONTRACT_M a
           LEFT JOIN (SELECT DISTINCT
                                NAME_0 AS SITE_CODE,
                                CCODE AS Company_Code
                       FROM     SYSTEM_PLANT,
                                SYSTEM_USER_PLANT
                       WHERE    SYSTEM_PLANT.LOCATION = SYSTEM_USER_PLANT.PLANT_LOCATION AND SYSTEM_PLANT.TYPE = SYSTEM_USER_PLANT.PLANT_TYPE ) b ON a.SITE_CODE = b.SITE_CODE
           LEFT JOIN dbo.CHANGE_HISTORY c
           ON a.CONTRACT_M_UID = c.OBJ_UID
           LEFT JOIN dbo.SYSTEM_USERS d
           ON a.APPLICANT = d.ACCOUNT
           WHERE CONTRACT_NO NOT IN ('HUA-2015-0393-IE')
           AND  c.NT_ACCOUNT IS NOT NULL
           AND d.ACCOUNT  IS NOT NULL
           AND   a.SITE_CODE + '#' + a.DEPARTMENT   IN(SELECT DISTINCT
                                                        SITE_CODE + '#' + DEPARTMENT AS col
                                               FROM     WF_REVIEW_TEAM_CONTRACT_SITE
                                               UNION ALL
                                               SELECT DISTINCT
                                                        SITE_NAME + '#' + DEPARTMENT AS col
                                               FROM     SYSTEM_User_DEPARTMENT)
";







            List<Insert_Contract_M_1> spp_contract_m = new List<Insert_Contract_M_1>();
            List<SPP.Econtract2._0.Contract_M> Contract_M_list = new List<SPP.Econtract2._0.Contract_M>();
            using (var context = new SPP_ProductionEntities())
            {
                spp_contract_m = context.Database.SqlQuery<Insert_Contract_M_1>(sql).ToList();
                foreach (var item in spp_contract_m)
                {
                    //try
                    //{
                    //    item.Applicant_UID = users.Where(m => m.User_NTID.ToLower() == item.APPLICANT.ToLower()).FirstOrDefault().Users_UID.ToString();
                    //}
                    //catch
                    //{
                    //    //item.Applicant_UID = modified_guid.ToString();
                    //    item.Applicant_UID = string.Empty;
                    //}


                    item.Applicant_UID = users.Where(m => m.User_NTID.ToLower() == item.APPLICANT.ToLower()).FirstOrDefault().Users_UID.ToString();
                    if (item.CONTRACT_TYPE == "Service-Employment/Head Hunter/Training/Internship Agreement")
                    {
                        item.CONTRACT_TYPE = "Employment/Head Hunter/Training/Internship Agreement";
                    }

                    item.JabilEntity_UID = company.Where(m => m.Company_Code == item.Company_Code).FirstOrDefault().Company_UID.ToString();
                    item.ContractType_D_UID = contract_d.Where(m => m.ContractType_D_Name_EN == item.CONTRACT_TYPE).FirstOrDefault().ContractType_D_UID.ToString();


                    item.Department_UID = department.Where(m => m.Company_UID == new Guid(item.JabilEntity_UID) & m.Department_Name == item.DEPARTMENT).FirstOrDefault().Department_UID.ToString();
                    //if (item.IS_RENEW.HasValue)
                    //{
                    //    if (Convert.ToInt32(item.IS_RENEW) == 1 || Convert.ToInt32(item.IS_RENEW) == 0)
                    //    {
                    //        item.Is_Renew_UID = typecode.Where(m => m.TypeCode_L2_Name == "New Contract").FirstOrDefault().TypeCode_L2_UID.ToString();
                    //    }
                    //    if (Convert.ToInt32(item.IS_RENEW) == 2)
                    //    {
                    //        item.Is_Renew_UID = typecode.Where(m => m.TypeCode_L2_Name == "Renew with same contract").FirstOrDefault().TypeCode_L2_UID.ToString();
                    //    }
                    //    if (Convert.ToInt32(item.IS_RENEW) == 3)
                    //    {
                    //        item.Is_Renew_UID = typecode.Where(m => m.TypeCode_L2_Name == "Renew with differ contract").FirstOrDefault().TypeCode_L2_UID.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    item.Is_Renew_UID = typecode.Where(m => m.TypeCode_L2_Name == "New Contract").FirstOrDefault().TypeCode_L2_UID.ToString();
                    //}


                    Contract_M model_Contract_M = new Contract_M();
                    model_Contract_M.Contract_M_UID = new Guid(item.CONTRACT_M_UID);
                    model_Contract_M.Contract_No = item.CONTRACT_NO.Trim();
                    model_Contract_M.Applicant_UID = new Guid(item.Applicant_UID);
                    model_Contract_M.JabilEntity_UID = new Guid(item.JabilEntity_UID);
                    model_Contract_M.Department_UID = new Guid(item.Department_UID);
                    model_Contract_M.ContractType_D_UID = new Guid(item.ContractType_D_UID);
                    if (item.IS_TEMPLATE_USED.HasValue)
                    {
                        model_Contract_M.Is_Template_Used = Convert.ToBoolean(item.IS_TEMPLATE_USED);
                    }
                    else
                    {
                        model_Contract_M.Is_Template_Used = false;
                    }

                    model_Contract_M.Contract_CostCenter = String.Empty;
                    model_Contract_M.Contract_Name = item.CONTRACT_NAME;
                    model_Contract_M.Contract_Desc = item.CONTRACT_DESC;
                    model_Contract_M.Vendor_Code = String.Empty;
                    model_Contract_M.Customer_Code = String.Empty;
                    model_Contract_M.Contractor = item.CONTRACTOR;
                    //model_Contract_M.Is_Renew = Convert.ToInt32(item.IS_RENEW);
                    model_Contract_M.Is_Renew = item.IS_RENEW.HasValue ? Convert.ToInt32(item.IS_RENEW) : 1;
                    model_Contract_M.Previous_Contract_No = String.Empty;
                    model_Contract_M.Copies = item.CONTRACT_COPIES;
                    if (item.CONTRACT_VALUE != 0)
                    {
                        model_Contract_M.With_Payment = true;
                    }
                    else
                    {
                        model_Contract_M.With_Payment = false;

                    }

                    model_Contract_M.Currency = item.CURRENCY_CODE;
                    model_Contract_M.Payment_Amount = item.CONTRACT_VALUE;
                    model_Contract_M.With_VAT = null;
                    model_Contract_M.Amount_Involve_Per_Year = null;
                    model_Contract_M.Amount_Involve_perYear_USD = null;
                    model_Contract_M.Exchange_Rate_toUSD = null;
                    model_Contract_M.Payment_Schedule = null;
                    model_Contract_M.Begin_Date = item.PERIOD_FROM;
                    model_Contract_M.End_Date = item.PERIOD_TO;
                    model_Contract_M.Expiration_Notice_Date = item.EXPIRATION_NOTICE_DATE;
                    model_Contract_M.Delivery_Date = item.DELIVERY_DATE;
                    model_Contract_M.Project_Commencement_Date = item.PRO_COMPLETION_DATE;
                    model_Contract_M.Project_Completion_Date = item.PRO_COMPLETION_DATE;
                    model_Contract_M.Warranty_Begin_Date = null;
                    model_Contract_M.Warranty_End_Date = item.WARRANTY_PERIOD;
                    model_Contract_M.Estimate_Effective_Date = item.ESTIMATE_EFFECTIVE_DATE;
                    model_Contract_M.Is_MultipleContractorMaster = null;
                    //model_Contract_M.Status = item.STATUS;
                    model_Contract_M.Status = GetStatus(item.STATUS);
                    model_Contract_M.Is_Enable = true;
                    if (model_Contract_M.Status.Trim() == ContractConsts.NewReviewStatus.Withdraw)
                    {
                        model_Contract_M.Is_Enable = false;
                    }
                    model_Contract_M.Version = item.VERSION;
                    model_Contract_M.Is_Latest = Convert.ToBoolean(item.IS_LATEST);
                    model_Contract_M.Modified_UID = users.Where(m => m.User_NTID.ToLower() == item.Modify_NTID.ToLower()).FirstOrDefault().Users_UID;
                    //try
                    //{
                    //    model_Contract_M.Modified_UID = users.Where(m => m.User_NTID == item.Modify_NTID).FirstOrDefault().Users_UID;
                    //}
                    //catch
                    //{
                    //    //item.Applicant_UID = string.Empty;
                    //    //model_Contract_M.Modified_UID = modified_guid;
                    //    model_Contract_M.Modified_UID = Guid.Empty;
                    //}


                    model_Contract_M.Modified_Date = item.MODIFY_DATETIME;
                    model_Contract_M.Modified_Remarks = modify_remarks;
                    model_Contract_M.CPT_No = String.Empty;
                    model_Contract_M.SRM_No = String.Empty;

                    Contract_M_list.Add(model_Contract_M);



                }

            }


            //using (var context = new SPP_MVC_Entities())
            //{
            //    context.Contract_M.AddRange(Contract_M_list);
            //    context.SaveChanges();
            //}

            BulkInsert(GetConnectString(), "Contract_M", Contract_M_list);

            endMethodName("Insert_Contract_M");

        }

        public static void Insert_Users_Role()
        {
            List<Users_Role> user_roles = new List<Users_Role>();

            user_roles.AddRange(Insert_Users_Role_Child(ContractConsts.OldUserRole.CONTRACT_CREATOR, ContractConsts.NewUserRole.Contract_Creator));
            user_roles.AddRange(Insert_Users_Role_Child(ContractConsts.OldUserRole.CONTRACT_REVIEWER, ContractConsts.NewUserRole.Contract_Approver));
            user_roles.AddRange(Insert_Users_Role_Child(ContractConsts.OldUserRole.CONTRACT_VIEWER, ContractConsts.NewUserRole.Contract_Viewer));
            user_roles.AddRange(Insert_Users_Role_Child(ContractConsts.OldUserRole.OP_ASSISTANT, ContractConsts.NewUserRole.E_Contract_OPA));
            user_roles.AddRange(Insert_Users_Role_Child(ContractConsts.OldUserRole.CONTRACT_ADMIN, ContractConsts.NewUserRole.E_Contract_Admin));
            BulkInsert(GetConnectString(), "Users_Role", user_roles);

            //            #region Contract Open Pending Closed -------------------Add By Hongzhong 2017/05
            //            using (var context = new SPP_MVC_Entities())
            //            {
            //                var sql_creator = @"
            //                        INSERT Users_Role
            //                        SELECT NEWID(),t.* FROM 
            //                        (
            //                        SELECT DISTINCT 
            //                        Applicant_UID ,
            //                        (SELECT  TOP 1 Role_UID FROM  ROLE WHERE Role_ID='Contract_Creator') AS Users_UID,
            //                        '0B08C006-5AB5-E611-83F5-005056BF221C' AS  Modified_UID,
            //                        GETDATE() AS Modified_Date, 
            //                        'E-Contract 1.0 data migration to E-Contract 2.0' AS Modified_Remarks
            //                         FROM Contract_M WHERE Is_latest=1
            //                        AND Applicant_UID NOT IN 
            //                        (
            //                        SELECT DISTINCT Users_UID FROM Users_Role WHERE Role_UID =
            //                        (SELECT  TOP 1 Role_UID FROM  ROLE WHERE Role_ID='Contract_Creator'))
            //                        )t
            //                        ";
            //                context.Database.ExecuteSqlCommand(sql_creator);
            //                context.SaveChanges();
            //            }
            //            #endregion

            //            #region Reviewer-------------------Add By Hongzhong 2017/05
            //            List<string> reivewer_ntid = new List<string>();
            //            using (var context = new SPP_ProductionEntities())
            //            {
            //                string sql = string.Format(@"
            //                SELECT DISTINCT LOWER(RTRIM(b.ACCOUNT)) AS account FROM dbo.SYSTEM_USERS_ROLE a INNER JOIN dbo.SYSTEM_USERS b
            //                ON a.ACCOUNT_UID=b.ACCOUNT_UID
            //                WHERE ROLE='CONTRACT_REVIEWER'
            //              ");
            //                reivewer_ntid = context.Database.SqlQuery<string>(sql).ToList<string>();
            //            }

            //            List<string> reviewer_exists = new List<string>();

            //            List<string> except_lists = new List<string>();
            //            using (var context = new SPP_MVC_Entities())
            //            {
            //                string sql = string.Format(@"SELECT LOWER(b.User_NTID) FROM dbo.Users_Role a INNER JOIN  dbo.Users b
            //ON a.Users_UID=b.Users_UID AND a.Role_UID=(SELECT Role_UID FROM dbo.Role WHERE Role_ID='Contract_Approver')
            //");
            //                reviewer_exists = context.Database.SqlQuery<string>(sql).ToList<string>();

            //                except_lists = reivewer_ntid.Except(reviewer_exists).ToList<string>();

            //                var users = context.Users.ToList();

            //                var approver_uid = context.Role.ToList().Where(m => m.Role_ID == "Contract_Approver").FirstOrDefault().Role_UID;

            //                List<Users_Role> user_roles = new List<Users_Role>();
            //                for (int i = 0; i < except_lists.Count; i++)
            //                {
            //                    Users_Role model_Users_Role = new Users_Role();
            //                    model_Users_Role.Users_Role_UID = Guid.NewGuid();
            //                    model_Users_Role.Users_UID = users.Where(m => m.User_NTID.ToLower() == except_lists[i].ToLower().ToString().Trim()).FirstOrDefault().Users_UID;
            //                    model_Users_Role.Role_UID = approver_uid;//
            //                    model_Users_Role.Modified_UID = modified_guid;
            //                    model_Users_Role.Modified_Date = DateTime.Now;
            //                    model_Users_Role.Modified_Remarks = modify_remarks;
            //                    user_roles.Add(model_Users_Role);

            //                }
            //                context.Users_Role.AddRange(user_roles);
            //                context.SaveChanges();
            //            }
            //            #endregion
            //            //User View 角色导入
            //            #region Contract_Viewer-------------------Add By Hongzhong 2017/05
            //            List<string> viewer_id = new List<string>();
            //            using (var context = new SPP_ProductionEntities())
            //            {
            //                string sql = string.Format(@"
            //                SELECT DISTINCT LOWER(RTRIM(b.ACCOUNT)) AS account FROM dbo.SYSTEM_USERS_ROLE a INNER JOIN dbo.SYSTEM_USERS b
            //                ON a.ACCOUNT_UID=b.ACCOUNT_UID
            //                WHERE ROLE='CONTRACT_VIEWER'
            //              ");
            //                viewer_id = context.Database.SqlQuery<string>(sql).ToList<string>();
            //            }
            //            List<string> viewer_exists = new List<string>();
            //            List<string> except_viewer_lists = new List<string>();
            //            using (var context = new SPP_MVC_Entities())
            //            {
            //                string sql = string.Format(@"SELECT LOWER(b.User_NTID) FROM dbo.Users_Role a INNER JOIN  dbo.Users b
            //ON a.Users_UID=b.Users_UID AND a.Role_UID=(SELECT Role_UID FROM dbo.Role WHERE Role_ID='Contract_Viewer')
            //");
            //                viewer_exists = context.Database.SqlQuery<string>(sql).ToList<string>();

            //                except_viewer_lists = viewer_id.Except(viewer_exists).ToList<string>();

            //                var users = context.Users.ToList();

            //                var viewer_uid = context.Role.Where(m => m.Role_ID == "Contract_Viewer").FirstOrDefault().Role_UID;

            //                List<Users_Role> user_roles = new List<Users_Role>();
            //                for (int i = 0; i < except_viewer_lists.Count; i++)
            //                {
            //                    Users_Role model_Users_Role = new Users_Role();
            //                    model_Users_Role.Users_Role_UID = Guid.NewGuid();
            //                    model_Users_Role.Users_UID = users.Where(m => m.User_NTID.ToLower() == except_viewer_lists[i].ToLower().ToString().Trim()).FirstOrDefault().Users_UID;
            //                    model_Users_Role.Role_UID = viewer_uid;//
            //                    model_Users_Role.Modified_UID = modified_guid;
            //                    model_Users_Role.Modified_Date = DateTime.Now;
            //                    model_Users_Role.Modified_Remarks = modify_remarks;
            //                    user_roles.Add(model_Users_Role);
            //                }
            //                context.Users_Role.AddRange(user_roles);
            //                context.SaveChanges();
            //            }
            //            #endregion





        }

        public static List<Users_Role> Insert_Users_Role_Child(string oldRoleName, string newRoleName)
        {

            List<string> ntid_lists = new List<string>();
            List<string> exists_lists = new List<string>();
            List<string> except_lists = new List<string>();
            using (var context = new SPP_ProductionEntities())
            {
                string sql = string.Format(@"
     SELECT DISTINCT LOWER(RTRIM(b.ACCOUNT)) AS account FROM dbo.SYSTEM_USERS_ROLE a INNER JOIN dbo.SYSTEM_USERS b
                ON a.ACCOUNT_UID=b.ACCOUNT_UID
                WHERE ROLE='{0}'
              ", oldRoleName);
                ntid_lists = context.Database.SqlQuery<string>(sql).ToList<string>();
            }


            using (var context = new SPP_MVC_Entities())
            {
                string sql = string.Format(@"SELECT LOWER(b.User_NTID) FROM dbo.Users_Role a INNER JOIN  dbo.Users b
ON a.Users_UID=b.Users_UID AND a.Role_UID=(SELECT Role_UID FROM dbo.Role WHERE Role_ID='{0}')
", newRoleName);
                exists_lists = context.Database.SqlQuery<string>(sql).ToList<string>();

                except_lists = ntid_lists.Except(exists_lists).ToList<string>();

                var users = context.Users.ToList();

                var approver_uid = context.Role.ToList().Where(m => m.Role_ID == newRoleName).FirstOrDefault().Role_UID;

                List<Users_Role> user_roles = new List<Users_Role>();
                for (int i = 0; i < except_lists.Count; i++)
                {
                    Users_Role model_Users_Role = new Users_Role();
                    model_Users_Role.Users_Role_UID = Guid.NewGuid();
                    model_Users_Role.Users_UID = users.Where(m => m.User_NTID.ToLower() == except_lists[i].ToLower().ToString().Trim()).FirstOrDefault().Users_UID;
                    model_Users_Role.Role_UID = approver_uid;//
                    model_Users_Role.Modified_UID = modified_guid;
                    model_Users_Role.Modified_Date = DateTime.Now;
                    model_Users_Role.Modified_Remarks = modify_remarks;
                    user_roles.Add(model_Users_Role);

                }

                return user_roles;
                //context.Users_Role.AddRange(user_roles);
                //context.SaveChanges();

            }

        }

        public static void Insert_Tb_Contract_Attachment()
        {
            starMethodName("Insert_Tb_Contract_Attachment");

            List<WF_REVIEW_TEAM_CONTRACT_SITE> reviewTeam = new List<WF_REVIEW_TEAM_CONTRACT_SITE>();
            List<CONTRACT_D> contract_d = new List<CONTRACT_D>();
            //            var sql = @"
            //SELECT a.*,b.ACCOUNT FROM CONTRACT_D a
            //LEFT JOIN dbo.SYSTEM_USERS b ON a.CREATOR=b.ACCOUNT 
            //WHERE CONTRACT_M_UID IN  (SELECT CONTRACT_M_UID FROM dbo.CONTRACT_M where  CONTRACT_TYPE <> '' AND CONTRACT_NO NOT IN ( '09361602031', '09361604008', '09361604028', '09361604030', 'GPB1512002', 'HUA-2015-0393-IE' ) ) AND b.ACCOUNT IS NOT NULL 
            //"
            //;

            //            var sql = @"
            //SELECT  *
            //FROM    CONTRACT_D
            //WHERE   CONTRACT_M_UID IN ( SELECT  CONTRACT_M_UID
            //                            FROM    dbo.CONTRACT_M a LEFT JOIN  dbo.SYSTEM_USERS b ON
            //                            a.APPLICANT=b.ACCOUNT
            //                            LEFT JOIN  dbo.CHANGE_HISTORY c
            //                            ON a.APPLICANT=c.NT_ACCOUNT
            //                            WHERE   CONTRACT_TYPE <> '' AND CONTRACT_NO NOT IN ( '09361602031', '09361604008', '09361604028', '09361604030', 'GPB1512002', 'HUA-2015-0393-IE' )
            //                            AND b.ACCOUNT IS NOT NULL
            //                            AND d.NT_ACCOUNT IS NOT NULL
            //                             )
            //";

            //            var sql = @"
            //SELECT  *
            //FROM    CONTRACT_D
            //WHERE   CONTRACT_M_UID IN ( SELECT DISTINCT CONTRACT_M_UID
            //                            FROM    dbo.CONTRACT_M a INNER JOIN  dbo.SYSTEM_USERS b ON
            //                            a.APPLICANT=b.ACCOUNT

            //                            WHERE  
            // CONTRACT_TYPE <> '' AND CONTRACT_NO NOT IN ( '09361602031', '09361604008', '09361604028', '09361604030', 'GPB1512002', 'HUA-2015-0393-IE' )   AND 
            //                          b.ACCOUNT IS NOT NULL

            //                             )
            //";


            var sql = @"
SELECT  *
FROM    CONTRACT_D
WHERE   CONTRACT_M_UID IN ( SELECT DISTINCT CONTRACT_M_UID
                            FROM    dbo.CONTRACT_M a INNER JOIN  dbo.SYSTEM_USERS b ON
                            a.APPLICANT=b.ACCOUNT
                      
                            WHERE  
 CONTRACT_NO NOT IN ('HUA-2015-0393-IE' )   AND 
                          b.ACCOUNT IS NOT NULL
                           
                             )
";



            List<Guid> contract_M_uid_str = new List<Guid>();
            using (var context = new SPP_MVC_Entities())
            {
                contract_M_uid_str = context.Contract_M.Select(m => m.Contract_M_UID).ToList<Guid>();

            }



            using (var context = new SPP_ProductionEntities())
            {
                reviewTeam = context.WF_REVIEW_TEAM_CONTRACT_SITE.ToList();
                contract_d = context.Database.SqlQuery<CONTRACT_D>(sql).ToList();

                contract_d = contract_d.Where(m => contract_M_uid_str.Contains(new Guid(m.CONTRACT_M_UID))).ToList();

            }

            List<Users> users = new List<Users>();
            List<Contract_Attachment> attachment_list = new List<Contract_Attachment>();
            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.ToList();

                //var contract_m_all = context.Contract_M.ToList();

                foreach (var item in contract_d)
                {
                    Contract_Attachment model_Contract_Attachment = new Contract_Attachment();
                    model_Contract_Attachment.Contract_Attachment_UID = new Guid(item.CONTRACT_D_UID);
                    model_Contract_Attachment.Contract_M_UID = new Guid(item.CONTRACT_M_UID);
                    model_Contract_Attachment.Attachment_Type = item.TEMPLATE_TYPE;
                    model_Contract_Attachment.System_File_Name = item.FILE_NAME;
                    model_Contract_Attachment.Original_File_Name = item.FILE_NAME;
                    model_Contract_Attachment.Display_File_Name = item.FILE_NAME;
                    model_Contract_Attachment.File_Size = 0;
                    model_Contract_Attachment.Tempkey = Guid.Empty;
                    model_Contract_Attachment.Uploaded_UID = users.Where(m => m.User_NTID.ToLower() == item.CREATOR.ToLower()).FirstOrDefault().Users_UID;
                    model_Contract_Attachment.Uploaded_Date = Convert.ToDateTime(item.UPLOAD_DATE);
                    var newFileName = model_Contract_Attachment.Uploaded_Date.ToString("yyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + ".x";
                    var newFilePath = string.Format(@"Temp/{0}", newFileName);
                    model_Contract_Attachment.System_File_Name = newFileName;//导入改名称
                    model_Contract_Attachment.File_Path = @"FileVault/" + newFileName;
                    attachment_list.Add(model_Contract_Attachment);

                }

                context.Contract_Attachment.AddRange(attachment_list);
                //context.SaveChanges();

                BulkInsert(GetConnectString(), "Contract_Attachment", attachment_list);
                endMethodName("Insert_Tb_Contract_Attachment");

            }

        }

        private static string ConvertToNtids(string ntids, List<Users> users)
        {
            string re = string.Empty;
            if (!string.IsNullOrEmpty(ntids))
            {
                var array = ntids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < array.Count; i++)
                {

                    if (i == array.Count - 1)
                    {
                        re = re + users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID.ToString();
                    }
                    else
                    {
                        re = re + users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID.ToString() + ",";

                    }
                }
            }


            return re;
        }

        private static List<Contract_WfTeam> InsertRoleUsers(Contract_WfTeam model, Insert_Tb_Contract_WfTeam_1 old_team, List<Users> users)
        {
            using (var context = new SPP_MVC_Entities())
            {
                List<Contract_WfTeam> Contract_WfTeam_List = new List<Contract_WfTeam>();

                #region Function Manager-------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.FM1))
                {

                    Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                    model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                    model_Contract_WfTeam.Department_UID = model.Department_UID;
                    model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                    model_Contract_WfTeam.Modified_UID = modified_guid;
                    model_Contract_WfTeam.Modified_Date = DateTime.Now;
                    model_Contract_WfTeam.Modified_Remarks = modify_remarks;

                    var user_uid = users.Where(m => m.User_NTID.ToLower() == old_team.FM1.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_Contract_WfTeam.Reviewer_UID = user_uid;
                    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Function_Manager_I;
                    Contract_WfTeam_List.Add(model_Contract_WfTeam);
                }
                if (!string.IsNullOrEmpty(old_team.FM2))
                {

                    Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                    model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                    model_Contract_WfTeam.Department_UID = model.Department_UID;
                    model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                    model_Contract_WfTeam.Modified_UID = modified_guid;
                    model_Contract_WfTeam.Modified_Date = DateTime.Now;
                    model_Contract_WfTeam.Modified_Remarks = modify_remarks;

                    var user_uid = users.Where(m => m.User_NTID.ToLower() == old_team.FM2.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_Contract_WfTeam.Reviewer_UID = user_uid;
                    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Function_Manager_II;
                    Contract_WfTeam_List.Add(model_Contract_WfTeam);

                }
                #endregion

                #region Purchasing -------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.PUR.ToString()))
                {

                    var array = old_team.PUR.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        //if (i > 0)
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Purchasing_II;
                        //}
                        //else
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Purchasing_I;
                        //}

                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Purchasing_I;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region SCM -------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.SCM.ToString()))
                {
                    var array = old_team.SCM.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        //if (i > 0)
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.SCM_II;
                        //}
                        //else
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.SCM_I;
                        //}

                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.SCM_I;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region Finance -------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.FINANCE.ToString()))
                {
                    var array = old_team.FINANCE.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Finance_I;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }

                if (!string.IsNullOrEmpty(old_team.FINANCE2.ToString()))
                {

                    var array = old_team.FINANCE2.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Finance_II;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region Legal -------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.LEGAL.ToString()))
                {
                    //var array = old_team.FINANCE2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var array = old_team.LEGAL.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Approver_I;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }


                if (!string.IsNullOrEmpty(old_team.C1ST_LEGAL_CUSTOMER.ToString()))
                {

                    //var array = old_team.FINANCE2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var array = old_team.C1ST_LEGAL_CUSTOMER.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Customer_I;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }

                if (!string.IsNullOrEmpty(old_team.C2ND_LEGAL_CUSTOMER.ToString()))
                {

                    var array = old_team.C2ND_LEGAL_CUSTOMER.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Customer_II;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }

                if (!string.IsNullOrEmpty(old_team.C1ST_LEGAL_SERVICE.ToString()))
                {

                    var array = old_team.C1ST_LEGAL_SERVICE.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Service_I;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }

                if (!string.IsNullOrEmpty(old_team.C2ND_LEGAL_SERVICE.ToString()))
                {

                    var array = old_team.C2ND_LEGAL_SERVICE.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Service_II;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }


                if (!string.IsNullOrEmpty(old_team.LEGAL_CUSTOMER_NDA.ToString()))
                {

                    var array = old_team.LEGAL_CUSTOMER_NDA.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Customer_NDA;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }


                //Legal_Customer_ABC
                if (!string.IsNullOrEmpty(old_team.C2ND_LEGAL_CUSTOMER.ToString()))
                {

                    var array = old_team.C2ND_LEGAL_CUSTOMER.Split(new char[] { ',' }).Distinct().ToList<string>();

                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Legal_Customer_ABC;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }


                #endregion

                #region OPM -------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.OPM.ToString()))
                {
                    var array = old_team.OPM.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        //if (i > 0)
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.OPD;
                        //}
                        //else
                        //{
                        //    model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.OPM;

                        //}

                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.OPM;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region OPA-------------------Add By Hongzhong 2017/05
                if (!string.IsNullOrEmpty(old_team.OPA.ToString()))
                {
                    var array = old_team.OPA.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.OP_Assistant;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region DCC -------------------Add By Hongzhong 2017/05

                if (!string.IsNullOrEmpty(old_team.DCC.ToString()))
                {
                    var array = old_team.DCC.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.File_In_PIC;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }
                #endregion

                #region Upload User -------------------Add By Hongzhong 2017/05
                var uploaderUser = old_team.SUBMIT + "," + old_team.OPA + "," + old_team.DCC;
                {
                    var array = uploaderUser.Split(new char[] { ',' }).Distinct().ToList<string>();
                    for (int i = 0; i < array.Count; i++)
                    {
                        Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                        model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                        model_Contract_WfTeam.Department_UID = model.Department_UID;
                        model_Contract_WfTeam.Submitter_UID = model.Submitter_UID;
                        model_Contract_WfTeam.Modified_UID = modified_guid;
                        model_Contract_WfTeam.Modified_Date = DateTime.Now;
                        model_Contract_WfTeam.Modified_Remarks = modify_remarks;
                        model_Contract_WfTeam.WfTask_Role = ContractConsts.RoleName.Upload_PIC;
                        var user_uid = users.Where(m => m.User_NTID.ToLower() == array[i].ToString().Trim().ToLower()).FirstOrDefault().Users_UID;
                        model_Contract_WfTeam.Reviewer_UID = user_uid;
                        Contract_WfTeam_List.Add(model_Contract_WfTeam);
                    }
                }


                #endregion

                return Contract_WfTeam_List;
            }


        }

        public static void Insert_Tb_Contract_WfTeam()
        {
            starMethodName("Insert_Tb_Contract_WfTeam");
            List<Insert_Tb_Contract_WfTeam_1> wf_review_teams = new List<Insert_Tb_Contract_WfTeam_1>();
            using (var context = new SPP_ProductionEntities())
            {
                var sql = @"
                           SELECT b.CCODE,'' AS Contract_WfTeam_UID,'' AS Department_UID ,'' AS Submit_UID, [SITE_CODE], [DEPARTMENT], [SUBMIT], [FM1], [FM2], [FINANCE], [LEGAL], [FINANCE2], [PUR], [OPM], [OPA], [SCM], [DCC], [1ST_LEGAL_CUSTOMER] AS C1ST_LEGAL_CUSTOMER, [2ND_LEGAL_CUSTOMER] AS C2ND_LEGAL_CUSTOMER, [1ST_LEGAL_SERVICE] AS C1ST_LEGAL_SERVICE, [2ND_LEGAL_SERVICE] AS C2ND_LEGAL_SERVICE, [LEGAL_CUSTOMER_NDA], [COST_CENTER]  FROM dbo.WF_REVIEW_TEAM_CONTRACT_SITE a
                           LEFT JOIN dbo.SYSTEM_PLANT b ON
                           a.SITE_CODE=b.NAME_0 
                           ";
                wf_review_teams = context.Database.SqlQuery<Insert_Tb_Contract_WfTeam_1>(sql).ToList();
            }
            List<Users> users = new List<Users>();
            List<Department> department = new List<Department>();
            List<Company> company = new List<Company>();
            List<Contract_WfTeam> list = new List<Contract_WfTeam>();
            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.ToList();
                department = context.Department.ToList();
                company = context.Company.ToList();
                foreach (var item in wf_review_teams)
                {
                    var company_uid = company.Where(m => m.Company_Code == item.CCODE).FirstOrDefault().Company_UID;

                    item.Department_UID = department.Where(m => m.Department_Name == item.DEPARTMENT & m.Company_UID == company_uid).FirstOrDefault().Department_UID.ToString();

                    item.Submit_UID = users.Where(m => m.User_NTID.ToLower() == item.SUBMIT.ToLower()).FirstOrDefault().Users_UID.ToString();

                    Contract_WfTeam model_Contract_WfTeam = new Contract_WfTeam();
                    model_Contract_WfTeam.Contract_WfTeam_UID = Guid.NewGuid();
                    model_Contract_WfTeam.Submitter_UID = new Guid(item.Submit_UID);
                    model_Contract_WfTeam.Department_UID = new Guid(item.Department_UID);
                    list.AddRange(InsertRoleUsers(model_Contract_WfTeam, item, users));
                }
                var date = DateTime.Now;
                list = list.Distinct(m => Tuple.Create(m.Department_UID, m.Submitter_UID, m.Reviewer_UID, m.WfTask_Role)).ToList();
                foreach (var item in list)
                {
                    item.Contract_WfTeam_UID = Guid.NewGuid();
                    item.Modified_Date = date;
                    item.Modified_UID = modified_guid;
                    item.Modified_Remarks = modify_remarks;
                }

                BulkInsert(GetConnectString(), "Contract_WfTeam", list);
                //context.Contract_WfTeam.AddRange(list);
                //context.Configuration.AutoDetectChangesEnabled = true;
                //context.Configuration.ValidateOnSaveEnabled = true;
                //context.SaveChanges();
                //context.Configuration.AutoDetectChangesEnabled = true;
                //context.Configuration.ValidateOnSaveEnabled = true;

                endMethodName("Insert_Tb_Contract_WfTeam");

            }

        }

        public static void Insert_Tb_WfTask()
        {
            starMethodName("Insert_Tb_WfTask");
            List<string> contract_M_no = new List<string>();
            List<Users> users = new List<Users>();
            using (var context = new SPP_MVC_Entities())
            {
                //History 表只保存Complete 、Withdraw的数据.
                contract_M_no = context.Contract_M.Select(m => m.Contract_No.ToString().ToLower().Trim()).Distinct().ToList<string>();
                users = context.Users.ToList();

            }

            List<WF_TASK> wf_old_task = new List<WF_TASK>();
            using (var context = new SPP_ProductionEntities())
            {
                //context.Database.CommandTimeout = 60 * 5;
                //wf_old_task = context.WF_TASK.Where(m => contract_M_uid_str.Contains(m.UID.ToLower()) && string.IsNullOrEmpty(m.STATE)).ToList();
                //wf_old_task = context.WF_TASK.Where(m => m.STATE == null).ToList();

                var sql = @"
SELECT * FROM dbo.WF_TASK WHERE
OBJ_NO  IN (SELECT DISTINCT  CONTRACT_NO FROM  dbo.CONTRACT_M WHERE STATUS NOT IN ('Completed','Withdraw') AND IS_LATEST=1 )

               ";
                wf_old_task = context.Database.SqlQuery<WF_TASK>(sql).ToList();

                wf_old_task = wf_old_task.Where(m => contract_M_no.Contains(m.OBJ_NO.ToLower().Trim())).ToList();
                //wf_old_task = context.WF_TASK.ToList().Where(m => contract_M_uid_str.Contains(new Guid(m.UID)) & m.STATE != null).ToList();

            }

            using (var context = new SPP_MVC_Entities())
            {
                //context.Database.CommandTimeout = 60 * 5;
                List<WfTask> WfTask_list = new List<WfTask>();

                foreach (var item in wf_old_task)
                {

                    WfTask model_WfTask_History = new WfTask();
                    var newUsers = users.Where(m => m.User_NTID.ToLower() == item.OWNER.Trim().ToLower()).FirstOrDefault();
                    if (newUsers != null)
                    {
                        model_WfTask_History.WfTask_UID = new Guid(item.UID);
                        model_WfTask_History.Module_UID = module_uid;
                        model_WfTask_History.Obj_No = item.OBJ_NO.Trim();
                        model_WfTask_History.Task_Name = item.TASK;
                        model_WfTask_History.Task_Role = GetRoleName(item.ROLE);
                        model_WfTask_History.Task_Owner = newUsers.Users_UID;
                        model_WfTask_History.State = item.STATE;
                        //model_WfTask_History.Comments = item.COMMENTS;
                        if (!string.IsNullOrEmpty(item.COMMENTS))
                        {
                            if (item.COMMENTS.Length < 500)
                            {
                                model_WfTask_History.Comments = item.COMMENTS;
                            }
                            else
                            {

                                model_WfTask_History.Comments = item.COMMENTS.Substring(0, 500);
                            }
                        }
                        model_WfTask_History.Assigned_Date = Convert.ToDateTime(item.ASSIGN_DATETIME);
                        model_WfTask_History.Remarks = item.REMARK;
                        model_WfTask_History.Completed_Date = item.COMPLETE_DATETIME;
                        model_WfTask_History.Return_Times = Convert.ToInt32(item.REVIEW_LOOP);
                        if (!string.IsNullOrEmpty(item.DELEGATE_FROM))
                        {
                            model_WfTask_History.Delegation_From_UID = users.Where(m => m.User_NTID.Trim().ToLower() == item.DELEGATE_FROM.Trim().ToLower()).FirstOrDefault().Users_UID;
                        }
                        model_WfTask_History.Resubmit_Routing = null;
                        WfTask_list.Add(model_WfTask_History);
                    }



                }
                BulkInsert(GetConnectString(), "WfTask", WfTask_list);
            }
            endMethodName("Insert_Tb_WfTask");

        }

        public static void Update_WFTask()
        {
            starMethodName("Update_WFTask");
            using (var context = new SPP_MVC_Entities())
            {
                var del_sql = string.Format(@"
                
UPDATE WfTask  SET  State='Cancel' , Comments='Canceled by System Admin, due to E-Contract 1.0 data migration to E-Contract 2.0.',
Completed_Date=GETDATE() WHERE WfTask_UID IN
(
SELECT WfTask_UID FROM  dbo.WfTask WHERE Task_Name='Review' AND (Task_Role <> 'OPM'  AND  Task_Role<>'Function Manager I' AND Task_Role<>'Function Manager II') AND State IS NULL  AND Module_UID='{0}'  
)

              ", module_uid);

                var insert_sql = @"
SELECT  NEWID() AS WfTask_UID ,
        t.*
FROM    ( SELECT DISTINCT
                    a.[Module_UID] ,
                    a.[Obj_No] ,
                    a.[Task_Name] ,
                    c.WfTask_Role AS Task_Role ,
                    c.Reviewer_UID AS Task_Owner ,
                    a.[State] ,
                    a.[Comments] ,
                    a.[Assigned_Date] ,
                    a.[Completed_Date] ,
                    a.[Return_Times] ,
                    a.[Delegation_From_UID] ,
                    a.[Resubmit_Routing] ,
                    a.[Remarks]
          FROM      dbo.WfTask a
          LEFT JOIN dbo.Contract_M b ON a.Obj_No = b.Contract_No
          LEFT JOIN dbo.Contract_WfTeam c ON b.Applicant_UID = c.Submitter_UID
          WHERE     a.Task_Name = 'Review' AND (
                    a.Task_Role <> 'OPM' AND a.Task_Role <> 'Function Manager I' AND a.Task_Role <> 'Function Manager II' AND  a.State IS NULL ) AND c.WfTask_Role = 'Function Manager I' ) t
                  
";

                var new_wftask = context.Database.SqlQuery<WfTask>(insert_sql).ToList();
                context.WfTask.AddRange(new_wftask);
                context.Database.ExecuteSqlCommand(del_sql);
                context.SaveChanges();


            }
            endMethodName("Update_WFTask");
        }

        public static void Update_Tb_Contract_Attachment()
        {
            starMethodName("Update_Tb_Contract_Attachment");
            string updatesql = @"

           ;WITH cte_one
           AS 
           (
           SELECT  Contract_Attachment_UID,Display_File_Name FROM 
           (
           SELECT  a.Contract_Attachment_UID ,a.Contract_M_UID , a.Attachment_Type AS  Attachment_Type , RTRIM(b.Contract_No)+'_'+ b.Contractor+'_'+b.CONTRACT_NAME+'_Draft Version'+ '.'+REVERSE(SUBSTRING(REVERSE(a.Original_File_Name), 0, CHARINDEX('.', REVERSE(Original_File_Name))))  AS Display_File_Name
           ,ROW_NUMBER() OVER (PARTITION BY a.Contract_M_UID,a.Attachment_Type ORDER BY GETDATE() ) AS rn 
            FROM Contract_Attachment a
           INNER JOIN Contract_M b
           ON a.Contract_M_UID =b.Contract_M_UID AND a.Attachment_Type=1
           ) t WHERE t.rn=1
           
           ) 
           UPDATE  a  SET a.Display_File_Name=b.Display_File_Name FROM   Contract_Attachment a INNER JOIN cte_one b ON a.Contract_Attachment_UID =b.Contract_Attachment_UID
          
           
           
           
           ;WITH cte_two
           AS
           (
           SELECT a.Contract_Attachment_UID , RTRIM(b.Contract_No)+'_'+ b.Contractor+'_'+b.CONTRACT_NAME+ '_Approval Version'+'.'+REVERSE(SUBSTRING(REVERSE(a.Original_File_Name), 0, CHARINDEX('.', REVERSE(Original_File_Name)))) AS Display_File_Name  FROM Contract_Attachment a
           INNER JOIN Contract_M b
           ON a.Contract_M_UID =b.Contract_M_UID
           WHERE a.Attachment_Type=3
           ) 
           UPDATE  a  SET a.Display_File_Name=b.Display_File_Name FROM   Contract_Attachment a INNER JOIN cte_two b ON a.Contract_Attachment_UID =b.Contract_Attachment_UID
           
          
           ;WITH cte_three
           AS
           (
           SELECT a.Contract_Attachment_UID , RTRIM(b.Contract_No)+'_'+ b.Contractor+'_'+b.CONTRACT_NAME+ '_Final Version'+'.'+REVERSE(SUBSTRING(REVERSE(a.Original_File_Name), 0, CHARINDEX('.', REVERSE(Original_File_Name)))) AS Display_File_Name  FROM Contract_Attachment a
           INNER JOIN Contract_M b
           ON a.Contract_M_UID =b.Contract_M_UID
           WHERE a.Attachment_Type=4
           )
           UPDATE  a  SET a.Display_File_Name=b.Display_File_Name FROM   Contract_Attachment a INNER JOIN cte_three b ON a.Contract_Attachment_UID =b.Contract_Attachment_UID
";



            using (var context = new SPP_MVC_Entities())
            {
                context.Database.ExecuteSqlCommand(updatesql);
                context.SaveChanges();

            }
            starMethodName("Update_Tb_Contract_Attachment");


        }

        public static void Insert_Tb_WfTask_History()
        {
            starMethodName("Insert_Tb_WfTask_History");
            List<string> contract_M_no = new List<string>();
            List<Users> users = new List<Users>();
            using (var context = new SPP_MVC_Entities())
            {
                contract_M_no = context.Contract_M.Select(m => m.Contract_No.ToString().ToLower().TrimEnd().TrimStart()).Distinct().ToList<string>();
                users = context.Users.ToList();

            }

            List<WF_TASK> wf_old_task = new List<WF_TASK>();
            using (var context = new SPP_ProductionEntities())
            {

                //var sql = @"
                //            SELECT [UID]
                //                  ,LTRIM(RTRIM([OBJ_NO])) AS [OBJ_NO]
                //                  ,[TASK]
                //                  ,[ROLE]
                //                  ,[OWNER]
                //                  ,[STATE]
                //                  ,[COMMENTS]
                //                  ,[ASSIGN_DATETIME]
                //                  ,[COMPLETE_DATETIME]
                //                  ,[REVIEW_LOOP]
                //                  ,[REMARK]
                //                  ,[DELEGATE_FROM]
                //                  ,[Send_Mail_Date]
                //              FROM [dbo].[WF_TASK]  
                //            WHERE  STATE IS NOT NULL AND  COMPLETE_DATETIME IS NOT NULL AND  ASSIGN_DATETIME IS NOT NULL  
                //                           ";

                var sql = @"
                         SELECT * FROM dbo.WF_TASK WHERE
                         OBJ_NO  IN (SELECT DISTINCT  CONTRACT_NO FROM  dbo.CONTRACT_M WHERE STATUS  IN ('Completed','Withdraw') AND IS_LATEST=1 )
                         ";

                wf_old_task = context.Database.SqlQuery<WF_TASK>(sql).ToList();




            }

            using (var context = new SPP_MVC_Entities())
            {
                //context.Database.CommandTimeout = 180 * 1000000;
                List<WfTask_History> WfTask_History_list = new List<WfTask_History>();

                foreach (var item in wf_old_task)
                {
                    //if (contract_M_no.Contains(item.OBJ_NO.ToLower().Trim()))
                    //{
                    var newuser = users.Where(m => m.User_NTID.ToLower().Trim() == item.OWNER.ToLower().Trim()).FirstOrDefault();
                    if (newuser != null)
                    {

                        WfTask_History model_WfTask_History = new WfTask_History();

                        Guid guid;
                        try
                        {
                            guid = new Guid(item.UID);
                        }
                        catch
                        {
                            guid = Guid.NewGuid();
                        }
                        model_WfTask_History.WfTask_History_UID = guid;
                        model_WfTask_History.Module_UID = module_uid;
                        model_WfTask_History.Obj_No = item.OBJ_NO.Trim();
                        model_WfTask_History.Task_Name = item.TASK;
                        model_WfTask_History.Task_Role = GetRoleName(item.ROLE);
                        model_WfTask_History.Task_Owner = users.Where(m => m.User_NTID.ToLower().Trim() == item.OWNER.ToLower().Trim()).FirstOrDefault().Users_UID;
                        model_WfTask_History.State = item.STATE;
                        if (!string.IsNullOrEmpty(item.COMMENTS))
                        {
                            if (item.COMMENTS.Length < 500)
                            {
                                model_WfTask_History.Comments = item.COMMENTS;
                            }
                            else
                            {

                                model_WfTask_History.Comments = item.COMMENTS.Substring(0, 500);
                            }
                        }


                        model_WfTask_History.Assigned_Date = Convert.ToDateTime(item.ASSIGN_DATETIME);
                        model_WfTask_History.Completed_Date = Convert.ToDateTime(item.COMPLETE_DATETIME);
                        model_WfTask_History.Return_Times = Convert.ToInt32(item.REVIEW_LOOP);
                        if (item.DELEGATE_FROM != null && !string.IsNullOrEmpty(item.DELEGATE_FROM))
                        {
                            model_WfTask_History.Delegation_From_UID = newuser.Users_UID;
                        }

                        model_WfTask_History.Backup_Date = DateTime.Now;
                        model_WfTask_History.Resubmit_Routing = null;
                        model_WfTask_History.Remarks = item.REMARK;
                        WfTask_History_list.Add(model_WfTask_History);

                    }


                    //}


                }
                BulkInsert(GetConnectString(), "WfTask_History", WfTask_History_list);

                //context.WfTask_History.AddRange(WfTask_History_list);


                //try
                //{
                //    context.SaveChanges();
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                //{
                //    var Re = string.Empty;
                //    foreach (var errors in ex.EntityValidationErrors)
                //    {
                //        foreach (var item in errors.ValidationErrors)
                //        {
                //            Re = Re + (item.ErrorMessage + item.PropertyName);
                //        }
                //    }

                //}

            }
            endMethodName("Insert_Tb_WfTask_History");

        }

        public static void Insert_Tb_WfTaskDelaySetting()
        {

            string sql = @"

delete from WfTaskDelaySetting

INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('DB491B44-5ADD-46F8-AD8B-9B2A6F3DB739', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'File In', 'File In PIC', 7, 'Applicant,Function Manager I', '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract DCC File In, notify applicant and applicant''s FM')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('CB86F4D1-1CA7-48A0-834C-62D7FECB24BD', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'ReadyToStamp', 'OP Assistant', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract OP Assistant ready to stamp')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('F362B6C9-E79F-4995-B975-6AD214636D65', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Finance I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Finance Manager Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('19B00EBF-BBA9-4940-B392-FC70CE499C92', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Finance II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Finance Controller Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('F07774E6-9282-46A2-99AC-8A845A49B220', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Function Manager I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Function Manager Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('3AB59CCE-0C72-445D-8172-338E71E14814', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Function Manager II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Function Manager II Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('514176F7-0F7F-4FB5-BF77-0A4B418D3CFE', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Approver I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('D0F8657E-0A8A-48A7-9B73-D19218F0FD62', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Approver II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal II Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('A7F5D170-3821-42A3-B1A8-997C3FAA594E', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Customer ABC', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('5B03634D-2880-4B72-857E-7F36BEDD1F46', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Customer I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('CA5E04E0-2B2B-4275-9510-083C2D7ADA1E', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Customer II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('6CD7D374-69FC-45D9-BE33-F5C6BA641113', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Customer NDA', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('0A0E40AE-6CBF-4E12-8708-A329BD2251DF', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Service I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('D519D2EA-7052-4058-9AF4-C11A1766DB54', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Legal Service II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Legal Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('277DC484-1C73-4C1D-9FB1-6BF1AE0DFB26', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'OPD', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract OPM/OPD Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('945D1BEC-E023-441E-9769-39F8605FFCA6', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'OPM', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract OPM/OPD Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('18C6F3F6-122C-4F9F-99F3-E7C0484C42B5', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Purchasing I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Purchasing Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('D7ABE05A-6348-4F6A-8686-FD473D5E5432', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'Purchasing II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Purchasing Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('FA2B84A2-7916-4397-BA60-ED40CBC57080', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'SCM I', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract SCM Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('BA5D5D63-D82B-44B1-89B5-2871A132F3B7', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Review', 'SCM II', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract SCM Review')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('243AEB97-DB57-4E52-B453-D1B9D690DA31', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'StampingDone', 'OP Assistant', 3, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract OP Assistant stamping')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('32256176-FB6A-4DBD-ACF8-AAB022D3C64B', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Submit', 'Applicant', 30, NULL, '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'Contract State is Rejected or Revoked')


INSERT INTO dbo.WfTaskDelaySetting (WfTaskDelaySetting_UID, Module_UID, Task_Name, Task_Role, Delay_Days, Reminder, Modified_UID, Modified_Date, Modified_Remarks)
VALUES ('0E3D1462-E847-4163-BFD1-C7E679341787', '39326F1E-54C6-4ED7-9D2D-A0415F8321D3', 'Upload', 'Upload PIC', 7, 'Upload PIC,Upload PIC FM', '0B08C006-5AB5-E611-83F5-005056BF221C', '2017-05-17 13:52:16.947', 'E-Contract Applicant/OPA/DCC Upload Final Verion')


        
";

            //List<WF_TASK_DELAY_CONFIG> delay_config = new List<WF_TASK_DELAY_CONFIG>();
            //using (var context = new SPP_ProductionEntities())
            //{
            //    //delay_config = context.WF_TASK_DELAY_CONFIG.Where(m => m.ROLE.Trim() != "OPA1" && m.ROLE.Trim() != "Applicant2").ToList();


            //    delay_config = context.WF_TASK_DELAY_CONFIG.ToList();
            //    foreach (var item in delay_config)
            //    {
            //        if (item.ROLE.Trim() == "Applicant2" && item.TASK == "Recheck")
            //        {
            //            item.ROLE = ContractConsts.RoleName.Applicant;
            //            item.TASK = ContractConsts.Task.File_In;
            //        }

            //        if (item.ROLE.Trim() == "Applicant" && item.TASK == "Recheck")
            //        {
            //            item.ROLE = ContractConsts.RoleName.Applicant;
            //            item.TASK = ContractConsts.Task.Submit;
            //        }
            //    }
            //}

            //List<WfTaskDelaySetting> delay_setting_list = new List<WfTaskDelaySetting>();

            starMethodName("Insert_Tb_WfTaskDelaySetting");
            using (var context = new SPP_MVC_Entities())
            {
                context.Database.ExecuteSqlCommand(sql);
                context.SaveChanges();

                //foreach (var item in delay_config)
                //{
                //    WfTaskDelaySetting model_WfTaskDelaySetting = new WfTaskDelaySetting();
                //    model_WfTaskDelaySetting.WfTaskDelaySetting_UID = Guid.NewGuid();
                //    model_WfTaskDelaySetting.Module_UID = module_uid;
                //    model_WfTaskDelaySetting.Task_Name = item.TASK;
                //    model_WfTaskDelaySetting.Task_Role = GetRoleName(item.ROLE.Trim());
                //    model_WfTaskDelaySetting.Delay_Days = Convert.ToInt32(item.DELAY_DAYS);
                //    model_WfTaskDelaySetting.Reminder = item.REMINDER;

                //    if (!string.IsNullOrEmpty(item.REMINDER))
                //    {
                //        var list = item.REMINDER.Split(',').ToList();


                //        for (int i = 0; i < list.Count; i++)
                //        {
                //            if (string.IsNullOrEmpty(list[i].ToString().Trim()))
                //            {
                //                list.Remove(list[i]);
                //            }
                //            else
                //            {
                //                list[i] = GetRoleName(list[i].ToString().Trim());
                //            }

                //        }

                //        model_WfTaskDelaySetting.Reminder = string.Join(",", list);
                //    }

                //    model_WfTaskDelaySetting.Modified_UID = modified_guid;
                //    model_WfTaskDelaySetting.Modified_Date = DateTime.Now;
                //    model_WfTaskDelaySetting.Modified_Remarks = item.REMARKS;
                //    delay_setting_list.Add(model_WfTaskDelaySetting);

                //}
                //context.WfTaskDelaySetting.AddRange(delay_setting_list);
                //context.SaveChanges();
            }

            endMethodName("Insert_Tb_WfTaskDelaySetting");


        }

        public static void Insert_Tb_WfDelegation()
        {
            List<CONTRACT_DELEGATION> CONTRACT_DELEGATION_list = new List<CONTRACT_DELEGATION>();

            using (var context = new SPP_ProductionEntities())
            {
                CONTRACT_DELEGATION_list = context.CONTRACT_DELEGATION.ToList();

            }

            List<WfDelegation> WfDelegation_list = new List<WfDelegation>();
            List<Users> users = new List<Users>();
            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.ToList();
                foreach (var item in CONTRACT_DELEGATION_list)
                {
                    WfDelegation model_WfDelegation = new WfDelegation();
                    model_WfDelegation.WfDelegation_UID = Guid.NewGuid();
                    model_WfDelegation.Module_UID = module_uid;
                    model_WfDelegation.Principal_UID = users.Where(m => m.User_NTID.Trim().ToLower() == item.CURRENT_USER.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_WfDelegation.Deputy_UID = users.Where(m => m.User_NTID.Trim().ToLower() == item.DEPUTY_USER.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_WfDelegation.Begin_Date = item.START_DATE;
                    model_WfDelegation.End_Date = item.END_DATE;
                    model_WfDelegation.Modified_UID = modified_guid;
                    model_WfDelegation.Modified_Date = DateTime.Now;
                    model_WfDelegation.Modified_Remarks = item.REMARK;
                    WfDelegation_list.Add(model_WfDelegation);

                }
                //WfDelegation_list.AddRange(model_WfDelegation);
                context.WfDelegation.AddRange(WfDelegation_list);
                context.SaveChanges();
            }

        }

        public static void Insert_Tb_WfDelegation_History()
        {
            List<CONTRACT_DELEGATION_BACKUP> CONTRACT_DELEGATION_list = new List<CONTRACT_DELEGATION_BACKUP>();

            using (var context = new SPP_ProductionEntities())
            {
                CONTRACT_DELEGATION_list = context.CONTRACT_DELEGATION_BACKUP.ToList();

            }

            List<WfDelegation_History> WfDelegation_list = new List<WfDelegation_History>();
            List<Users> users = new List<Users>();
            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.ToList();
                foreach (var item in CONTRACT_DELEGATION_list)
                {
                    WfDelegation_History model_WfDelegation = new WfDelegation_History();
                    model_WfDelegation.WfDelegation_History_UID = Guid.NewGuid();
                    model_WfDelegation.Module_UID = module_uid;
                    model_WfDelegation.Principal_UID = users.Where(m => m.User_NTID.Trim().ToLower() == item.CURRENT_USER.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_WfDelegation.Deputy_UID = users.Where(m => m.User_NTID.Trim().ToLower() == item.DEPUTY_USER.Trim().ToLower()).FirstOrDefault().Users_UID;
                    model_WfDelegation.Begin_Date = item.START_DATE;
                    model_WfDelegation.End_Date = item.END_DATE;
                    model_WfDelegation.Modified_UID = modified_guid;
                    model_WfDelegation.Modified_Date = DateTime.Now;
                    model_WfDelegation.Modified_Remarks = item.REMARK;
                    WfDelegation_list.Add(model_WfDelegation);

                }
                //WfDelegation_list.AddRange(model_WfDelegation);
                context.WfDelegation_History.AddRange(WfDelegation_list);
                context.SaveChanges();
            }


        }

        public static void Insert_Tb_WfEmail_StopExpirationNotice()
        {

            List<CONTRACT_EXPIRATION_NOTICE> CONTRACT_EXPIRATION_NOTICE_list = new List<CONTRACT_EXPIRATION_NOTICE>();
            using (var context = new SPP_ProductionEntities())
            {
                CONTRACT_EXPIRATION_NOTICE_list = context.CONTRACT_EXPIRATION_NOTICE.ToList();

            }

            List<WfEmail_StopExpirationNotice> WfEmail_StopExpirationNotice_list = new List<WfEmail_StopExpirationNotice>();
            using (var context = new SPP_MVC_Entities())
            {

                foreach (var item in CONTRACT_EXPIRATION_NOTICE_list)
                {
                    WfEmail_StopExpirationNotice model_WfEmail_StopExpirationNotice = new WfEmail_StopExpirationNotice();
                    model_WfEmail_StopExpirationNotice.WfEmail_StopExpirationNotice_UID = Guid.NewGuid();
                    model_WfEmail_StopExpirationNotice.Obj_Table = "Contract_M";
                    model_WfEmail_StopExpirationNotice.Obj_No = item.CONTRACT_NO;
                    model_WfEmail_StopExpirationNotice.Is_Renew = Convert.ToBoolean(item.IS_RENEW);
                    model_WfEmail_StopExpirationNotice.Modified_UID = modified_guid;
                    model_WfEmail_StopExpirationNotice.Modified_Date = DateTime.Now;
                    model_WfEmail_StopExpirationNotice.Modified_Remarks = String.Empty;
                    WfEmail_StopExpirationNotice_list.Add(model_WfEmail_StopExpirationNotice);

                }
                context.WfEmail_StopExpirationNotice.AddRange(WfEmail_StopExpirationNotice_list);
                context.SaveChanges();
            }



        }

        public static void MigrationProduction()
        {

            starMethodName("MigrationProduction");
            string sql = @"
---用户信息修正

UPDATE CONTRACT_M SET APPLICANT='linja' WHERE APPLICANT='Jackey Lin'
UPDATE dbo.WF_TASK SET OWNER='linja' WHERE OWNER='Jackey Lin'
UPDATE dbo.CHANGE_HISTORY  SET NT_ACCOUNT='linja' WHERE NT_ACCOUNT='Jackey Lin'

UPDATE CONTRACT_M SET APPLICANT='Wangy684' WHERE APPLICANT='wngy684'
UPDATE dbo.WF_TASK SET OWNER='Wangy684' WHERE OWNER='wngy684'
UPDATE dbo.CHANGE_HISTORY  SET NT_ACCOUNT='Wangy684' WHERE NT_ACCOUNT='wngy684'


--1.Contract No  Version 重复

--09361602031
DELETE FROM  dbo.CONTRACT_M WHERE CONTRACT_M_UID='fe96714b-e37f-48fc-bca0-6da8a015d62f'
DELETE FROM CONTRACT_D WHERE CONTRACT_M_UID='fe96714b-e37f-48fc-bca0-6da8a015d62f'
--09361604008
DELETE FROM  dbo.CONTRACT_M WHERE CONTRACT_M_UID='2a64df02-93e4-405f-9bbe-cb0158c5b0db'
DELETE FROM CONTRACT_D WHERE CONTRACT_M_UID='2a64df02-93e4-405f-9bbe-cb0158c5b0db'
--09361604028
DELETE FROM  dbo.CONTRACT_M WHERE CONTRACT_M_UID='99e01d31-96db-4b18-94ae-e055b00d5cce'
DELETE FROM CONTRACT_D WHERE CONTRACT_M_UID='99e01d31-96db-4b18-94ae-e055b00d5cce'
--09361604030
DELETE FROM  dbo.CONTRACT_M WHERE CONTRACT_M_UID='031e6e73-362a-4e37-af99-fb4b9cdbf30a'
DELETE FROM CONTRACT_D WHERE CONTRACT_M_UID='031e6e73-362a-4e37-af99-fb4b9cdbf30a'
--GPB1512002
DELETE FROM  dbo.CONTRACT_M WHERE CONTRACT_M_UID='1ba40cef-83cd-4cd4-97cd-7836dfb35769'
DELETE FROM CONTRACT_D WHERE CONTRACT_M_UID='1ba40cef-83cd-4cd4-97cd-7836dfb35769'

--2.Contract Type 为空
--- 09021608130 Existing contract upload in e-Contract system for record purpose.（不导入）
--- 09021608131 existing contract for filling purpose only（不导入）
---09491703030 wf_task 未有签核流程，（不导入）.
DELETE FROM dbo.CHANGE_HISTORY WHERE OBJ_UID IN (SELECT CONTRACT_M_UID FROM dbo.CONTRACT_M WHERE CONTRACT_NO IN ('09021608130','09021608131','09491703030'))
DELETE FROM dbo.CONTRACT_D WHERE CONTRACT_M_UID IN   (SELECT CONTRACT_M_UID FROM dbo.CONTRACT_M WHERE CONTRACT_NO IN ('09021608130','09021608131','09491703030'))
DELETE FROM dbo.WF_TASK WHERE OBJ_NO IN ( '09021608130','09021608131','09491703030')
DELETE FROM dbo.CONTRACT_M WHERE CONTRACT_NO IN ( '09021608130','09021608131','09491703030')
UPDATE CONTRACT_M  SET CONTRACT_TYPE='Contractor Service Agreement'  WHERE CONTRACT_NO='09041608013'

--3.找出Contract_M表中非GUID 字段并更新.转换成Guid
DECLARE @oldID VARCHAR(50)
SET @oldID='09131601021-v3'
DECLARE @NewGuid VARCHAR(50)
SET @NewGuid='5596BF04-E559-4CD5-9973-E8B1BA5E0476'
UPDATE CONTRACT_M SET CONTRACT_M_UID=@NewGuid WHERE CONTRACT_M_UID=@oldID
UPDATE CONTRACT_D SET CONTRACT_M_UID=@NewGuid WHERE CONTRACT_M_UID=@oldID
UPDATE CHANGE_HISTORY SET OBJ_UID=@NewGuid WHERE OBJ_UID=@oldID

DECLARE @oldID1 VARCHAR(50)
SET @oldID1='09131601049-v5'
DECLARE @NewGuid1 VARCHAR(50)
SET @NewGuid1='ECEE6777-0033-4375-A26B-56882A2030DC'
UPDATE CONTRACT_M SET CONTRACT_M_UID=@NewGuid1 WHERE CONTRACT_M_UID=@oldID1
UPDATE CONTRACT_D SET CONTRACT_M_UID=@NewGuid1 WHERE CONTRACT_M_UID=@oldID1
UPDATE CHANGE_HISTORY SET OBJ_UID=@NewGuid1 WHERE OBJ_UID=@oldID1


DECLARE @oldID2 VARCHAR(50)
SET @oldID2='09131602008-v3'
DECLARE @NewGuid2 VARCHAR(50)
SET @NewGuid2='42844D6E-9654-422B-9E25-2AAA23B60525'
UPDATE CONTRACT_M SET CONTRACT_M_UID=@NewGuid2 WHERE CONTRACT_M_UID=@oldID2
UPDATE CONTRACT_D SET CONTRACT_M_UID=@NewGuid2 WHERE CONTRACT_M_UID=@oldID2
UPDATE CHANGE_HISTORY SET OBJ_UID=@NewGuid2 WHERE OBJ_UID=@oldID2


DECLARE @oldID3 VARCHAR(50)
SET @oldID3='09131603017-v5'
DECLARE @NewGuid3 VARCHAR(50)
SET @NewGuid3='6E00EBA0-06CF-4322-B7BB-C0BACCA87CAD'
UPDATE CONTRACT_M SET CONTRACT_M_UID=@NewGuid3 WHERE CONTRACT_M_UID=@oldID3
UPDATE CONTRACT_D SET CONTRACT_M_UID=@NewGuid3 WHERE CONTRACT_M_UID=@oldID3
UPDATE CHANGE_HISTORY SET OBJ_UID=@NewGuid3 WHERE OBJ_UID=@oldID3


---4.找出Contract_D 表中非GUID 的字段，转成GUID
UPDATE CONTRACT_D SET CONTRACT_D_UID=NEWID() WHERE CONTRACT_D_UID IN (SELECT CONTRACT_D_UID FROM CONTRACT_D  WHERE  LEN(CONTRACT_D_UID) <>36)

---5.找出WF_task表中非GUID字段，更新成GUID
UPDATE WF_TASK SET UID=NEWID() WHERE LEN(UID)<>36

---6.修改重复的OBJ_NO,TASK,ROLE,OWNER,ASSIGN_DATETIME 时间.
UPDATE dbo.WF_TASK SET  ASSIGN_DATETIME=DATEADD(SECOND,1, ASSIGN_DATETIME)
WHERE uid IN
(
SELECT UID FROM 
(
SELECT  * ,ROW_NUMBER() OVER (PARTITION BY OBJ_NO,TASK,ROLE,OWNER,ASSIGN_DATETIME ORDER BY  GETDATE()) AS rn
 FROM  dbo.WF_TASK
 ) t WHERE  t.rn>1
 )

         ";
            using (var context = new SPP_ProductionEntities())
            {
                context.Database.ExecuteSqlCommand(sql);
                context.SaveChanges();

            }

            endMethodName("MigrationProduction");
        }

        public static void Contract_Attachment_ToCMD()
        {
            starMethodName("Contract_Attachment_ToCMD");
            using (var context = new SPP_MVC_Entities())
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT  'color 0a'".ToString());
                sb.AppendLine("UNION ALL".ToString());
                sb.AppendLine("SELECT  'echo %date% %time%'".ToString());
                sb.AppendLine("UNION ALL".ToString());
                sb.AppendLine("SELECT  'echo 正在批量重命名文件......'".ToString());
                sb.AppendLine("UNION ALL".ToString());
                sb.AppendLine("SELECT  'ren ' + '\"' + Original_File_Name + '\"' + ' ' + System_File_Name".ToString());
                sb.AppendLine("FROM    Contract_Attachment".ToString());
                sb.AppendLine("UNION ALL".ToString());
                sb.AppendLine("SELECT  'echo 重命名完成'".ToString());
                sb.AppendLine("UNION ALL".ToString());
                sb.AppendLine("SELECT  'pause'".ToString());
                sb.ToString();

                var list = context.Database.SqlQuery<string>(sb.ToString());
                StringBuilder sb2 = new StringBuilder();
                foreach (var item in list)
                {
                    sb2.AppendLine(item.ToString());
                }
                var last = sb2.ToString();
                byte[] bytes = Encoding.Default.GetBytes(last);
                LocalFileHelper.CreateFile("Contract_Attachment.cmd", bytes);
            }

            endMethodName("Contract_Attachment_ToCMD");

        }


       #region 测试数据 -------------------Add By Hongzhong 2017/04


        public static void test_Data()
        {
            //WF_TASK wf_task = new WF_TASK();

            //SPP_Produciton_BaseRepository<WF_TASK> w = new SPP_Produciton_BaseRepository<WF_TASK>();
            //w.LoadEntities(m => m.OBJ_NO != null).FirstOrDefault();
            //w.AddEntities();

            var ntid = "huange3";
            var role_id = "System_Admin,Contract_Creator";
            Users users = new Users();
            List<Role> roles = new List<Role>();






            using (var context = new SPP_MVC_Entities())
            {
                users = context.Users.Where(m => m.User_NTID == ntid).FirstOrDefault();
                //roles = context.Role.Where(m => m.Role_ID == role_id).FirstOrDefault();
                roles = context.Role.ToList();
                var list = role_id.Split(',').ToList<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    Users_Role model_Users_Role = new Users_Role();
                    model_Users_Role.Users_Role_UID = Guid.NewGuid();
                    model_Users_Role.Users_UID = users.Users_UID;
                    model_Users_Role.Role_UID = roles.Where(m => m.Role_ID == list[i].ToString()).FirstOrDefault().Role_UID;
                    model_Users_Role.Modified_UID = modified_guid;
                    model_Users_Role.Modified_Date = DateTime.Now;
                    model_Users_Role.Modified_Remarks = String.Empty;
                    context.Users_Role.Add(model_Users_Role);

                }



                //更新ContractTempate 数据.
                var sql = @"
UPDATE ContractTemplate SET Company_UID=(SELECT TOP  1 Company_UID FROM dbo.Company WHERE Company_Code='0953')
WHERE Company_UID=(SELECT  TOP 1 Company_UID FROM  ContractTemplate WHERE Company_UID=(SELECT TOP 1 Company_UID FROM dbo.Company WHERE Company_Code='0902')
)

";

                context.Database.ExecuteSqlCommand(sql);

                context.SaveChanges();

            }










        }

        public static void insert_huange3_role()
        {



        }

        #endregion

    }

}

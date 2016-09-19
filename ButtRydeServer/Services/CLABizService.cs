using AASC.FW.UnitOfWork;
using AASC.Partner.API.Configuration.Cla;
using AASC.Partner.API.ErrorHelpers;
using AASC.Partner.API.Helpers;
using AASC.Partner.API.Models;
using AASC.Partner.API.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;

namespace AASC.Partner.API.Services
{
    public class CLABizService : ICLABizService
    {
        protected readonly ICLADataService _claDataService;

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public CLABizService(
            IUnitOfWorkAsync unitOfWorkAsync,
            ICLADataService claService
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _claDataService = claService;
        }

        private IQueryable<CLAFormListViewModel> GetQueryable(string order)
        {
            var projection = _claDataService.Query().Select().AsQueryable().OrderBy(order).ToList();

            var data = new List<CLAFormListViewModel>();

            projection.ForEach(x =>
            {
                var d = ConvertFrom(x);
                data.Add(d);
            });

            return data.AsQueryable();
        }

        //public string ModelToString(List<IArrayThing> list) {
        //    string temp = "";
        //    foreach (var thing in list) {
        //        string.Concat(temp, thing.ToString());
        //    }
        //    return temp;
        //}

        public OperationResult<CLAFormViewModel> Create(CLAFormViewModel model)
        {
            string DeviceList = "[";
            string ProductQuantity = "[";
            foreach (var thing in model.DeviceCategories)
            {
                DeviceList = string.Concat(DeviceList, thing.ToString());
            }
            DeviceList = DeviceList.Substring(0, DeviceList.Length - 1);

            foreach (var thing in model.ProductList)
            {
                ProductQuantity = string.Concat(ProductQuantity, thing.ToString());
            }

            ProductQuantity = ProductQuantity.Substring(0, ProductQuantity.Length - 1);
            DeviceList = string.Concat(DeviceList, "]");
            ProductQuantity = string.Concat(ProductQuantity, "]");

            var claForm = new CLAForm()
            {

                Id = Guid.NewGuid().ToString(),
                CompanyName = model.CompanyName,
                TaxID = model.TaxID,
                SalesContact = model.SalesContact,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                PostCode = model.PostCode,
                SignerFirstName = model.SignerFirstName,
                SignerLastName = model.SignerLastName,
                SignerEmail = model.SignerEmail,
                SignerJobTitle = model.SignerJobTitle,
                SignerPhoneNumber = model.SignerPhoneNumber,
                TechnicalFirstName = model.TechnicalFirstName,
                TechnicalastName = model.TechnicalLastName,
                TechnicalEmail = model.TechnicalEmail,
                TechnicalJobTitle = model.TechnicalJobTitle,
                TechnicalPhoneNumber = model.TechnicalPhoneNumber,
                DeviceCategories = DeviceList,
                ProductList = ProductQuantity,
                OtherType = model.OtherType,
                OtherQuantity = model.OtherQuantity,
                CLAStatus = "User Submitted",
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _claDataService.Insert(claForm);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(claForm.Id).Data.FirstOrDefault();

                //// send out email
                //List<string> recipients = new List<string>();
                //var buyEmail = "eric.shih@advantech.com";
                //recipients.Add(buyEmail);

                ////var salesEmail = CLAConfig.getEmail(data.SalesContact);
                ////var pmEmail = CLAConfig.getPMEmail();
                ////var buyEmail = "buy@advantech.com";

                ////recipients.Add(salesEmail);
                ////recipients.Add(pmEmail);
                ////recipients.Add(buyEmail);

                //EmailServer emailServer = new EmailServer();
                //var body = GetEmailBody(data.Id);
                //emailServer.Send("CLA Form Application Notification", body, recipients);

                return new OperationResult<CLAFormViewModel>
                {
                    Data = ConvertFrom(data),
                    Status = OperationResult.Success,
                    Message = "CLA form saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CLAFormViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CLAFormViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<CLAFormViewModel> Delete(string id)
        {
            var claForm = _claDataService.Find(id);

            if (claForm == null)
            {
                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _claDataService.Delete(claForm.Id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Success,
                    Message = "Form deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<CLAFormListViewModel> Get(string id)
        {
            List<CLAFormListViewModel> results = new List<CLAFormListViewModel>();

            int total = 0;

            string order = "Id";

            var query = GetQueryable(order);

            var data = query.Where(x => string.Compare(x.Id, id, true) == 0).FirstOrDefault();

            results.Add(data);

            total = results.Count();

            return new Result<CLAFormListViewModel> { Data = results, Total = total };
        }


        public Result<CLAFormListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<CLAFormListViewModel> results = new List<CLAFormListViewModel>();

            int total = 0;

            string order = "Id";

            Filtering filtering = null;

            // order by
            if (sorting != null && sorting != "undefined" && sorting != "[]")
            {
                List<Sort> sort = JsonConvert.DeserializeObject<List<Sort>>(sorting);
                List<string> sorts = new List<string>();
                sort.ForEach(x => sorts.Add(string.Format("{0} {1}", x.field, x.dir)));
                order = string.Join(", ", sorts.ToArray());
            }

            var data = GetQueryable(order);

            // filtered by
            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                filtering = JsonConvert.DeserializeObject<Filtering>(filter);

                var predicate = new DynamicLinqHelper<CLAFormListViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<CLAFormListViewModel> { Data = results, Total = total };
            }
            else
            {

                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<CLAFormListViewModel> { Data = results, Total = total };
            }
        }

        public OperationResult<CLAFormViewModel> Update(CLAFormViewModel model)
        {
            var claForm = _claDataService.Find(model.Id);
            string DeviceList = "[";
            string ProductQuantity = "[";
            foreach (var thing in model.DeviceCategories)
            {
                DeviceList = string.Concat(DeviceList, thing.ToString());
            }
            DeviceList = DeviceList.Substring(0, DeviceList.Length - 1);

            foreach (var thing in model.ProductList)
            {
                ProductQuantity = string.Concat(ProductQuantity, thing.ToString());
            }

            ProductQuantity = ProductQuantity.Substring(0, ProductQuantity.Length - 1);
            DeviceList = string.Concat(DeviceList, "]");
            ProductQuantity = string.Concat(ProductQuantity, "]");
            if (claForm == null)
                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Failed,
                    Message = string.Format("Cla Form {0} does not exists", model.Id)
                };

            claForm.CompanyName = model.CompanyName;
            claForm.TaxID = model.TaxID;
            claForm.SalesContact = model.SalesContact;
            claForm.Address = model.Address;
            claForm.City = model.City;
            claForm.Country = model.Country;
            claForm.PostCode = model.PostCode;
            claForm.SignerFirstName = model.SignerFirstName;
            claForm.SignerLastName = model.SignerLastName;
            claForm.SignerEmail = model.SignerEmail;
            claForm.SignerJobTitle = model.SignerJobTitle;
            claForm.SignerPhoneNumber = model.SignerPhoneNumber;
            claForm.TechnicalFirstName = model.TechnicalFirstName;
            claForm.TechnicalastName = model.TechnicalLastName;
            claForm.TechnicalEmail = model.TechnicalEmail;
            claForm.TechnicalJobTitle = model.TechnicalJobTitle;
            claForm.TechnicalPhoneNumber = model.TechnicalPhoneNumber;
            claForm.DeviceCategories = DeviceList;
            claForm.ProductList = ProductQuantity;

            claForm.CLANumber = model.CLANumber;
            claForm.CLAStatus = model.CLAStatus;
            claForm.CLAStatusDate = model.CLAStatusDate;
            claForm.CustomerERPID = model.CustomerERPID;
            //CreatedById = createdById;
            //claForm.CreatedDate = DateTime.UtcNow;
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _claDataService.Update(claForm);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(model.Id).Data.FirstOrDefault();

                return new OperationResult<CLAFormViewModel>
                {
                    Data = ConvertFrom(data),
                    Status = OperationResult.Success,
                    Message = "CLA Form saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<CLAFormViewModel>
                {
                    Data = default(CLAFormViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<CLAFormListViewModel> GetDisplayList()
        {
            List<CLAFormListViewModel> results = new List<CLAFormListViewModel>();

            int total = 0;

            string order = "Id";

            var data = GetQueryable(order);

            //results.Add(default(CompanyViewModel));

            results.AddRange(data);

            total = results.Count();

            return new Result<CLAFormListViewModel> { Data = results, Total = total };
        }

        public string GetEmailBody(string id)
        {
            var url = "http://localhost:9573/claform/view/"; //change this to partners.advantech
            var body = string.Empty;
            var result = Get(id);
            body = "<div>";

            body += string.Format("<h1>CLA Form Email Receipt</h1>");

            body += string.Format("<h3><a title=\"this\" href=\"" + url + id + "\">Click here to view on website</a><h3>");

            foreach (var r in result.Data)
            {
                
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");
                ////////////
                body += string.Format("<tr>");
                body += string.Format("<th width='190'><div><b>General Information<b></div></th>");
                body += string.Format("<th width='270'></th>");
                body += string.Format("</tr>");
                
                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Sales Contact</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.SalesContact);
                body += string.Format("</tr>");

                body += string.Format("</table>");
                body += string.Format("<br />");
                ////////////

                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='190'><div><b>Company Information<b></div></th>");
                body += string.Format("<th width='270'></th>");
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Company Name</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.CompanyName);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Address</div></td>");
                body += string.Format("<td width='270'><div>{0} {1}, {2}</div></td>", r.Address, r.City, r.Country);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Post Code</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.PostCode);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Tax ID</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.TaxID);
                body += string.Format("</tr>");

                body += string.Format("</table>");



                body += string.Format("<br />");
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='190'><div><b>Signer Contact Info<b></div></th>");
                body += string.Format("<th width='270'></th>");
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Name</div></td>");
                body += string.Format("<td width='270'><div>{0} {1}</div></td>", r.SignerFirstName, r.SignerLastName);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Job Title</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerJobTitle);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Email</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerEmail);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Phone Number</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerPhoneNumber);
                body += string.Format("</tr>");

                body += string.Format("</table>");


                body += string.Format("<br />");
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='190'><div><b>Technical Contact Info<b></div></th>");
                body += string.Format("<th width='270'></th>");
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Name</div></td>");
                body += string.Format("<td width='270'><div>{0} {1}</div></td>", r.TechnicalFirstName, r.TechnicalLastName);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Job Title</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalJobTitle);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Email</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalEmail);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Phone Number</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalPhoneNumber);
                body += string.Format("</tr>");

                body += string.Format("</table>");



                body += string.Format("<br />");
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='270'><div><b>Device Categories<b></div></th>");
                body += string.Format("<th width='190'></th>");
                body += string.Format("</tr>");
                
                var deviceCategories = JsonConvert.DeserializeObject<List<DeviceSelected>>(r.DeviceCategories);
                foreach (var d in deviceCategories.Where(x => x.Selected == true))
                {
                    body += string.Format("<tr>");
                    body += string.Format("<td width='270'><div>{0}</div></td>", d.Device);
                    body += string.Format("<td width='190'>&#10003;</td>");
                    body += string.Format("</tr>");
                }
                
                body += string.Format("</table>");

                
                body += string.Format("<br />");
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='170'>Product</th>");
                body += string.Format("<th width='170'>Type</th>");
                body += string.Format("<th width='98'>Quantity</th>");
                body += string.Format("</tr>");

                var productList = JsonConvert.DeserializeObject<List<ProductChosen>>(r.ProductList);
                foreach (var d in productList.Where(x => x.Quantity > 0))
                {
                    body += string.Format("<tr>");
                    body += string.Format("<td width='170'><div>{0}</div></td>", d.ProductType);
                    body += string.Format("<td width='170'><div>{0}</div></td>", d.Product);
                    body += string.Format("<td width='98'><div>{0}</div></td>", d.Quantity);
                    body += string.Format("</tr>");
                }

                body += string.Format("<tr>");
                body += string.Format("<td width='170'><div>OTHER TYPE</div></td>");
                body += string.Format("<td width='170'><div>{0}</div></td>", r.OtherType);
                body += string.Format("<td width='98'><div>{0}</div></td>", r.OtherQuantity);
                body += string.Format("</tr>");
                body += string.Format("</table>");

                
                
                body += string.Format("<br />");
                body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

                body += string.Format("<tr>");
                body += string.Format("<th width='190'><div><b>CLA Information<b></div></th>");
                body += string.Format("<th width='270'></th>");
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>CLA Number</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.CLANumber);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>CLA Status</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.CLAStatus);
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>CLA Status Date</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", (r.CLAStatusDate == null ? "" : ((DateTime)r.CLAStatusDate).ToString("D")));
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Created Date</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.CreatedDate.ToString("D"));
                body += string.Format("</tr>");

                body += string.Format("<tr>");
                body += string.Format("<td width='190'><div>Customer ERPID</div></td>");
                body += string.Format("<td width='270'><div>{0}</div></td>", r.CustomerERPID);
                body += string.Format("</tr>");

                body += string.Format("</table>");
                
            }
            body += "</div>";
            return body;
        }

        private CLAFormListViewModel ConvertFrom(CLAForm claform)
        {
            // var data = DataMapper.Map<CLAForm, CLAFormViewModel>(claform);

            var data = new CLAFormListViewModel();
            data.Address = claform.Address;
            data.City = claform.City;
            data.CLANumber = claform.CLANumber;
            data.CLAStatus = claform.CLAStatus;
            data.CLAStatusDate = claform.CLAStatusDate;
            data.CompanyName = claform.CompanyName;
            data.Country = claform.Country;
            data.CreatedDate = claform.CreatedDate;
            data.CustomerERPID = claform.CustomerERPID;
            data.Id = claform.Id;
            data.OtherQuantity = claform.OtherQuantity;
            data.OtherType = claform.OtherType;
            data.PostCode = claform.PostCode;
            data.SalesContact = claform.SalesContact;
            data.SignerEmail = claform.SignerEmail;
            data.SignerFirstName = claform.SignerFirstName;
            data.SignerJobTitle = claform.SignerJobTitle;
            data.SignerLastName = claform.SignerLastName;
            data.SignerPhoneNumber = claform.SignerPhoneNumber;
            data.TaxID = claform.TaxID;
            data.TechnicalEmail = claform.TechnicalEmail;
            data.TechnicalFirstName = claform.TechnicalFirstName;
            data.TechnicalJobTitle = claform.TechnicalJobTitle;
            data.TechnicalLastName = claform.TechnicalastName;
            data.TechnicalPhoneNumber = claform.TechnicalPhoneNumber;

            

            // var deviceCategories = JsonConvert.DeserializeObject<List<DeviceSelected>>(claform.DeviceCategories);
            data.DeviceCategories = claform.DeviceCategories;

            //var productList = JsonConvert.DeserializeObject<List<ProductChosen>>(claform.ProductList);
            data.ProductList = claform.ProductList;
            return data;
        }
        private CLAFormViewModel ConvertFrom(CLAFormListViewModel claform)
        {
            // var data = DataMapper.Map<CLAForm, CLAFormViewModel>(claform);

            var data = new CLAFormViewModel();
            data.Address = claform.Address;
            data.City = claform.City;
            data.CLANumber = claform.CLANumber;
            data.CLAStatus = claform.CLAStatus;
            data.CLAStatusDate = claform.CLAStatusDate;
            data.CompanyName = claform.CompanyName;
            data.Country = claform.Country;
            data.CreatedDate = claform.CreatedDate;
            data.CustomerERPID = claform.CustomerERPID;
            data.Id = claform.Id;
            data.OtherQuantity = claform.OtherQuantity;
            data.OtherType = claform.OtherType;
            data.PostCode = claform.PostCode;
            data.SalesContact = claform.SalesContact;
            data.SignerEmail = claform.SignerEmail;
            data.SignerFirstName = claform.SignerFirstName;
            data.SignerJobTitle = claform.SignerJobTitle;
            data.SignerLastName = claform.SignerLastName;
            data.SignerPhoneNumber = claform.SignerPhoneNumber;
            data.TaxID = claform.TaxID;
            data.TechnicalEmail = claform.TechnicalEmail;
            data.TechnicalFirstName = claform.TechnicalFirstName;
            data.TechnicalJobTitle = claform.TechnicalJobTitle;
            data.TechnicalLastName = claform.TechnicalLastName;
            data.TechnicalPhoneNumber = claform.TechnicalPhoneNumber;

            var deviceCategories = JsonConvert.DeserializeObject<List<DeviceSelected>>(claform.DeviceCategories);
            data.DeviceCategories = deviceCategories;

            var productList = JsonConvert.DeserializeObject<List<ProductChosen>>(claform.ProductList);
            data.ProductList = productList;
            return data;
        }

        public OperationResult<CLAFormListViewModel> Create(CLAFormListViewModel model)
        {
            throw new NotImplementedException();
        }


        OperationResult<CLAFormListViewModel> ICLABizService.Delete(string id)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICLABizService
    {
        Result<CLAFormListViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter);

        Result<CLAFormListViewModel> Get(string id);

        Result<CLAFormListViewModel> GetDisplayList();

        OperationResult<CLAFormViewModel> Create(CLAFormViewModel model);

        OperationResult<CLAFormListViewModel> Create(CLAFormListViewModel model);

        OperationResult<CLAFormViewModel> Update(CLAFormViewModel model);

        OperationResult<CLAFormListViewModel> Delete(string id);

        string GetEmailBody(string id);

    }

}
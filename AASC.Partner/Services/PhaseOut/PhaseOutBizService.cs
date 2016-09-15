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
    public class PhaseOutPrepBizService : IPhaseOutPrepBizService
    {
        protected readonly IPhaseOutDataService _phaseOutDataService;

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public PhaseOutPrepBizService(
            IUnitOfWorkAsync unitOfWorkAsync,
            IPhaseOutDataService phaseOutService
        )
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _phaseOutDataService = phaseOutService;
        }

        private IQueryable<PhaseOutPrepViewModel> GetQueryable(string order)
        {
            var projection = _phaseOutDataService.Query().Select().AsQueryable().OrderBy(order).ToList();

            var data = new List<PhaseOutPrepViewModel>();

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

        public OperationResult<PhaseOutPrepViewModel> Create(PhaseOutPrepViewModel model)
        {
            var PhaseOut = new PhaseOutPrep()
            {

                Id = Guid.NewGuid().ToString(),
                Phased = model.Phased,
                PartNumber = model.PartNumber,
                Description = model.Description,
                PlmStatus = model.PlmStatus,
                ProductFamily = model.ProductFamily,
                LastBuyTime = model.LastBuyTime,
                Replacement = model.Replacement,
                CreatedBy = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _phaseOutDataService.Insert(PhaseOut);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(PhaseOut.Id).Data.FirstOrDefault();

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

                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "CLA form saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = null,
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public OperationResult<PhaseOutPrepViewModel> Delete(string id)
        {
            var claForm = _phaseOutDataService.Find(id);

            if (claForm == null)
            {
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.NotFound,
                    Message = "Not Found"
                };
            }

            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _phaseOutDataService.Delete(claForm.Id);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Success,
                    Message = "Form deleted."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        public Result<PhaseOutPrepViewModel> Get(string id)
        {
            List<PhaseOutPrepViewModel> results = new List<PhaseOutPrepViewModel>();

            int total = 0;

            string order = "Id";

            var query = GetQueryable(order);

            var data = query.Where(x => string.Compare(x.Id, id, true) == 0).FirstOrDefault();

            results.Add(data);

            total = results.Count();

            return new Result<PhaseOutPrepViewModel> { Data = results, Total = total };
        }


        public Result<PhaseOutPrepViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter)
        {
            List<PhaseOutPrepViewModel> results = new List<PhaseOutPrepViewModel>();

            int total = 0;

            string order = "Id";

            Filtering filtering = null;

            // order by
            if (sorting != null && sorting != "undefined")
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

                var predicate = new DynamicLinqHelper<PhaseOutPrepViewModel>().CreateFilterPredicate(filtering.filters, true);

                total = data.Where(predicate).Count();
                foreach (var d in data.Where(predicate).Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<PhaseOutPrepViewModel> { Data = results, Total = total };
            }
            else
            {

                total = data.Count();
                foreach (var d in data.Skip(skip).Take(take))
                {
                    results.Add(d);
                }
                return new Result<PhaseOutPrepViewModel> { Data = results, Total = total };
            }
        }

        public OperationResult<PhaseOutPrepViewModel> Update(PhaseOutPrepViewModel model)
        {
            var phaseOut = _phaseOutDataService.Find(model.Id);

            if (phaseOut == null)
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Failed,
                    Message = string.Format("Phase Out Item {0} does not exists", model.Id)
                };



            phaseOut.Id = Guid.NewGuid().ToString();
            phaseOut.Phased = model.Phased;
            phaseOut.PartNumber = model.PartNumber;
            phaseOut.Description = model.Description;
            phaseOut.PlmStatus = model.PlmStatus;
            phaseOut.ProductFamily = model.ProductFamily;
            phaseOut.LastBuyTime = model.LastBuyTime;
            phaseOut.Replacement = model.Replacement;
            phaseOut.CreatedBy = Guid.NewGuid().ToString();
            phaseOut.CreatedDate = DateTime.UtcNow;
            
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                _phaseOutDataService.Update(phaseOut);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();

                var data = Get(model.Id).Data.FirstOrDefault();

                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = data,
                    Status = OperationResult.Success,
                    Message = "Phase Out Item saved."
                };
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = DbEntiyValidationExceptionHelper.RetrieveMessage(ex);

                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Failed,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                return new OperationResult<PhaseOutPrepViewModel>
                {
                    Data = default(PhaseOutPrepViewModel),
                    Status = OperationResult.Failed,
                    Message = ex.Message + InnerExceptionHandler.Retrieve(ex)
                };
            }
        }

        //public Result<PhaseOutPrepViewModel> GetDisplayList()
        //{
        //    List<PhaseOutPrepViewModel> results = new List<PhaseOutPrepViewModel>();

        //    int total = 0;

        //    string order = "Id";

        //    var data = GetQueryable(order);

        //    //results.Add(default(CompanyViewModel));

        //    results.AddRange(data);

        //    total = results.Count();

        //    return new Result<PhaseOutPrepViewModel> { Data = results, Total = total };
        //}

        public string GetEmailBody(string id)
        {
        //    var url = "http://localhost:9573/claform/view/"; //change this to partners.advantech
            var body = string.Empty;
        //    var result = Get(id);
        //    body = "<div>";

        //    body += string.Format("<h1>CLA Form Email Receipt</h1>");

        //    body += string.Format("<h3><a title=\"this\" href=\"" + url + id + "\">Click here to view on website</a><h3>");

        //    foreach (var r in result.Data)
        //    {
                
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");
        //        //////////
        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='190'><div><b>General Information<b></div></th>");
        //        body += string.Format("<th width='270'></th>");
        //        body += string.Format("</tr>");
                
        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Sales Contact</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.SalesContact);
        //        body += string.Format("</tr>");

        //        body += string.Format("</table>");
        //        body += string.Format("<br />");
        //        //////////

        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='190'><div><b>Company Information<b></div></th>");
        //        body += string.Format("<th width='270'></th>");
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Company Name</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CompanyName);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Address</div></td>");
        //        body += string.Format("<td width='270'><div>{0} {1}, {2}</div></td>", r.Address, r.City, r.Country);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Post Code</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.PostCode);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Tax ID</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.TaxID);
        //        body += string.Format("</tr>");

        //        body += string.Format("</table>");



        //        body += string.Format("<br />");
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='190'><div><b>Signer Contact Info<b></div></th>");
        //        body += string.Format("<th width='270'></th>");
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Name</div></td>");
        //        body += string.Format("<td width='270'><div>{0} {1}</div></td>", r.SignerFirstName, r.SignerLastName);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Job Title</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerJobTitle);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Email</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerEmail);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Phone Number</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.SignerPhoneNumber);
        //        body += string.Format("</tr>");

        //        body += string.Format("</table>");


        //        body += string.Format("<br />");
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='190'><div><b>Technical Contact Info<b></div></th>");
        //        body += string.Format("<th width='270'></th>");
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Name</div></td>");
        //        body += string.Format("<td width='270'><div>{0} {1}</div></td>", r.TechnicalFirstName, r.TechnicalLastName);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Job Title</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalJobTitle);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Email</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalEmail);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Phone Number</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.TechnicalPhoneNumber);
        //        body += string.Format("</tr>");

        //        body += string.Format("</table>");



        //        body += string.Format("<br />");
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='270'><div><b>Device Categories<b></div></th>");
        //        body += string.Format("<th width='190'></th>");
        //        body += string.Format("</tr>");
                
        //        var deviceCategories = JsonConvert.DeserializeObject<List<DeviceSelected>>(r.DeviceCategories);
        //        foreach (var d in deviceCategories.Where(x => x.Selected == true))
        //        {
        //            body += string.Format("<tr>");
        //            body += string.Format("<td width='270'><div>{0}</div></td>", d.Device);
        //            body += string.Format("<td width='190'>&#10003;</td>");
        //            body += string.Format("</tr>");
        //        }
                
        //        body += string.Format("</table>");

                
        //        body += string.Format("<br />");
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='170'>Product</th>");
        //        body += string.Format("<th width='170'>Type</th>");
        //        body += string.Format("<th width='98'>Quantity</th>");
        //        body += string.Format("</tr>");

        //        var productList = JsonConvert.DeserializeObject<List<ProductChosen>>(r.ProductList);
        //        foreach (var d in productList.Where(x => x.Quantity > 0))
        //        {
        //            body += string.Format("<tr>");
        //            body += string.Format("<td width='170'><div>{0}</div></td>", d.ProductType);
        //            body += string.Format("<td width='170'><div>{0}</div></td>", d.Product);
        //            body += string.Format("<td width='98'><div>{0}</div></td>", d.Quantity);
        //            body += string.Format("</tr>");
        //        }

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='170'><div>OTHER TYPE</div></td>");
        //        body += string.Format("<td width='170'><div>{0}</div></td>", r.OtherType);
        //        body += string.Format("<td width='98'><div>{0}</div></td>", r.OtherQuantity);
        //        body += string.Format("</tr>");
        //        body += string.Format("</table>");

                
                
        //        body += string.Format("<br />");
        //        body += string.Format("<table cellspacing='0' cellpadding='10' border='1'>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<th width='190'><div><b>CLA Information<b></div></th>");
        //        body += string.Format("<th width='270'></th>");
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>CLA Number</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CLANumber);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>CLA Status</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CLAStatus);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>CLA Status Date</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CLAStatusDate);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Created Date</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CreatedDate);
        //        body += string.Format("</tr>");

        //        body += string.Format("<tr>");
        //        body += string.Format("<td width='190'><div>Customer ERPID</div></td>");
        //        body += string.Format("<td width='270'><div>{0}</div></td>", r.CustomerERPID);
        //        body += string.Format("</tr>");

        //        body += string.Format("</table>");
                
        //    }
        //    body += "</div>";
            return body;
        }

        private PhaseOutPrepViewModel ConvertFrom(PhaseOutPrep model)
        {
            // var data = DataMapper.Map<CLAForm, PhaseOutPrepViewModel>(claform);

            var phaseOut = new PhaseOutPrepViewModel();


            phaseOut.Id = Guid.NewGuid().ToString();
            phaseOut.Phased = model.Phased;
            phaseOut.PartNumber = model.PartNumber;
            phaseOut.Description = model.Description;
            phaseOut.PlmStatus = model.PlmStatus;
            phaseOut.ProductFamily = model.ProductFamily;
            phaseOut.LastBuyTime = model.LastBuyTime;
            phaseOut.Replacement = model.Replacement;
            phaseOut.CreatedBy = Guid.NewGuid().ToString();
            phaseOut.CreatedDate = DateTime.UtcNow.ToString();
            
            return phaseOut;
        }

        
    }

    public interface IPhaseOutPrepBizService
    {
        Result<PhaseOutPrepViewModel> Get(int pageSize, int page, int skip, int take, string sorting, string filter);

        Result<PhaseOutPrepViewModel> Get(string id);


        OperationResult<PhaseOutPrepViewModel> Create(PhaseOutPrepViewModel model);

        OperationResult<PhaseOutPrepViewModel> Update(PhaseOutPrepViewModel model);

        OperationResult<PhaseOutPrepViewModel> Delete(string id);

        string GetEmailBody(string id);

    }

}
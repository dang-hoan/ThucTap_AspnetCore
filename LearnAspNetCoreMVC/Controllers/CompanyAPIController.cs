using LearnAspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using LearnAspNetCoreMVC.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authorization;

namespace LearnAspNetCoreMVC.Controllers
{
    [ApiController]
    [Route("api/companyAPI")]
    public class CompanyAPIController : Controller
    {
        //protected APIResponse _response;
        //Fix bug
        //paging
        //custom message
        //authentication + authorization => login, logout + account table => role
        //mapper
        //redirection to getById method:  CreatedAtRoute("GetById", new { id = villa.Id }, _response);
        //viết hoa controller, cấu trúc một api
        //async
        //try catch

        private readonly IRepository<Company> _companyRepository;
        private readonly IMapper _iMapper;
        public CompanyAPIController(IRepository<Company> companyRepository, IMapper iMapper)
        {
            _companyRepository = companyRepository;
            _iMapper = iMapper;
        }

        //GET
        [HttpGet("GetCompany/{id:int?}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetCompany(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    Message = "Your request too bad! Please check id field again! (id > 0)"
                });
            }
            var obj = _companyRepository.GetById((int)id);
            if (obj == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    Message = "There is no any company match with passed id!"
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                Message = "Get company by id successfully!",
                Data = obj
            });
        }

        [HttpGet("getCompany")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetCompany(int? id, string? name, string? address, string? phoneNumber, int? pageSize = null, int? pageNumber = null)
        {
            //IEnumerable<Company> res = from company in _companyRepository.Get()
            //                           where (id == null || company.Id == id)
            //                           && (name == null || company.Name.Contains(name))
            //                           && (address == null || company.Address.Contains(address))
            //                           && (phoneNumber == null || company.PhoneNumber.Contains(phoneNumber))
            //                           let compareDate = company.UpdateDate != null ? company.UpdateDate : company.CreateDate
            //                           orderby compareDate descending, company.Name
            //                           select company;

            IQueryable<Company> data = _companyRepository.Get(company =>
                                        (id == null || company.Id == id) &&
                                        (name == null || company.Name.Contains(name)) &&
                                        (address == null || company.Address.Contains(address)) &&
                                        (phoneNumber == null || company.PhoneNumber.Contains(phoneNumber)), pageSize, pageNumber);
            try
            {
                if (!data.Any())
                {
                    return NotFound(new APIResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        IsSuccess = false,
                        Message = "Couldn't find any satisfied companies or there is an error occurred!"
                    });
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false,
                    Message = "There is an error in adding data to database!"
                });

            }

            IEnumerable<Company> res = from company in data
                                       let compareDate = company.UpdateDate != null ? company.UpdateDate : company.CreateDate
                                       orderby compareDate descending, company.Name
                                       select company;

            return Ok(new APIResponse
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                Message = "Get companies successfully!",
                Data = res
            });
        }

        //POST
        [HttpPost("Create")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] CompanyPatch patchObj)
        {
            if (ModelState.IsValid)
            {
                if (patchObj.Id != 0)
                {
                    return BadRequest(new APIResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccess = false,
                        Message = "Your request too bad! Create method don't need you to support id field! (Please set it to 0)"
                    });
                }

                Company obj = _iMapper.Map<Company>(patchObj);

                if (_companyRepository.Add(obj) > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, new APIResponse
                    {
                        StatusCode = StatusCodes.Status201Created,
                        IsSuccess = true,
                        Message = "Company created successfully!"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        IsSuccess = false,
                        Message = "There is an error in adding data to database!"
                    });
                }
            }
            return BadRequest(new APIResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccess = false,
                Message = "Your request too bad! Please check all fields again!"
            });
        }

        //POST
        [HttpPut("Edit")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Edit([FromBody] CompanyPatch patchObj)
        {
            if (patchObj.Id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    Message = "Your request too bad! Please check id field again (id > 0)!"
                });
            }

            var obj = _companyRepository.Get(company => company.Id == patchObj.Id);
            if (!obj.Any())
            {
                return NotFound(new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    Message = "There is no any company match with passed id!"
                });
            }

            var company = obj.FirstOrDefault();
            _iMapper.Map(patchObj, company);            
            if (ModelState.IsValid)
            {
                if (_companyRepository.Update(company) > 0)
                {
                    return Ok(new APIResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccess = true,
                        Message = "Company updated successfully!"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        IsSuccess = false,
                        Message = "There is an error in updating data to database!"
                    });
                }
            }
            return BadRequest(new APIResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccess = false,
                Message = "Your request too bad! Please check all fields again!"
            });
        }

        //POST
        //Ex:     "path": "/Name",
        //        "op": "replace",
        //        "value": "CTY"
        [HttpPut("EditPatch")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult EditPatch(int id, JsonPatchDocument<Company> patchObj)
        {
            if (patchObj == null || id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    Message = "Your request too bad! Please check all fields again!"
                });
            }

            var obj = _companyRepository.GetById(id);
            if (obj == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    Message = "There is no any company match with passed id!"
                });
            }

            try
            {
                foreach (var operation in patchObj.Operations)
                {
                    if (operation.path?.EndsWith("/id") == true || operation.path?.EndsWith("/Id") == true)
                    {
                        return BadRequest(new APIResponse
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            IsSuccess = false,
                            Message = "Your request too bad! Please not update id field!"
                        });
                    }
                }

                patchObj.ApplyTo(obj, ModelState);
                if (ModelState.IsValid)
                {
                    if (_companyRepository.Update(obj) > 0)
                    {
                        return Ok(new APIResponse
                        {
                            StatusCode = StatusCodes.Status200OK,
                            IsSuccess = true,
                            Message = "Company updated successfully!"
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            IsSuccess = false,
                            Message = "There is an error in updating data to database!"
                        });
                    }
                }
            }catch (Exception)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    Message = "Your request too bad! Please check all fields again!"
                });
            }
            return BadRequest(new APIResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccess = false,
                Message = "Your request too bad! Please check all fields again!"
            });
        }

        //POST
        [HttpDelete("Delete"), ActionName("Delete")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest(new APIResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    Message = "Your request too bad! Please check id field again (id > 0)!"
                });
            }

            var obj = _companyRepository.GetById((int)id);
            if (obj == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    Message = "There is no any company match with passed id!"
                });
            }

            if (ModelState.IsValid)
            {
                if (_companyRepository.Delete(obj) > 0)
                {
                    return Ok(new APIResponse
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccess = true,
                        Message = "Company deleted successfully!"
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        IsSuccess = false,
                        Message = "There is an error in deleting data to database!"
                    });
                }
            }

            return BadRequest(new APIResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccess = false,
                Message = "Your request too bad! Please check all fields again!"
            });
        }
    }
}
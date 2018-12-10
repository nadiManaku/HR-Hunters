﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRHunters.Common.Entities;
using HRHunters.Common.Enums;
using HRHunters.Common.Interfaces;
using HRHunters.Common.Responses.AdminDashboard;
using HRHunters.Data;
using HRHunters.Common.ExtensionMethods;

namespace HRHunters.Domain.Managers
{
    public class ApplicantManager : BaseManager, IApplicantManager
    {
        private readonly IRepository _repo;
        public ApplicantManager(IRepository repo) : base(repo)
        {
            _repo = repo;
        }
        public ApplicantResponse GetMultiple(int pageSize = 20, int currentPage = 1, string sortedBy = "", SortDirection sortDir = SortDirection.ASC, string filterBy = "", string filterQuery = "")
        {
            var response = new ApplicantResponse() { Applicant = new List<ApplicantInfo>()};
                var query = _repo.GetAll<Applicant>(
                    includeProperties: $"{nameof(Applicant.User)},")
                                        .Select(
                                        x =>  new ApplicantInfo
                                        {
                                            Id = x.UserId,
                                            FirstName = x.User.FirstName,
                                            LastName = x.User.LastName,
                                            Email = x.User.Email,
                                            PhoneNumber = x.PhoneNumber,
                                            Photo = "photo"
                                        })
                                        .Applyfilters(pageSize, currentPage, sortedBy, sortDir, filterBy, filterQuery)
                                        .ToList();
            response.Applicant.AddRange(query);
            response.MaxApplicants = _repo.GetCount<Applicant>();
            return response;
        }
    }

}

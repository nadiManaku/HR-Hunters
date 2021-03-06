﻿using HRHunters.Common.Entities;
using HRHunters.Common.Enums;
using HRHunters.Common.Requests;
using HRHunters.Common.Requests.Admin;
using HRHunters.Common.Requests.Users;
using HRHunters.Common.Responses;
using HRHunters.Common.Responses.AdminDashboard;
using HRHunters.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace HRHunters.Common.Interfaces
{
    public interface IJobManager : IBaseManager
    {
        Task<JobResponse> GetMultiple(SearchRequest request);
        Task<GeneralResponse> CreateJobPosting(JobSubmit jobSubmit, int currentUserId);
        GeneralResponse UploadCSV(FileUpload fileUpload);
        JobInfo GetOneJobPosting(int id);
        GeneralResponse UpdateJob(JobUpdate jobUpdate);

    }
}

using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class RevisionLogsRepository : IRevisionLogsRepository
    {
        private readonly db_telgeprojectContext _context;

        public RevisionLogsRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<RevisionLogsViewModel> revision, int TotalCount)> GetAllRevisionsAsync(int Id, string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var revisionQuery = from revision in _context.TblRevisions
                                    where revision.IsDeleted == false && revision.CreatedBy == userId
                                    select new RevisionLogsViewModel
                                    {
                                        RevisionId = revision.RevisionId,
                                        ProjectId = revision.ProjectId,
                                        ScopeOfWork = revision.ScopeOfWork,
                                        Reason = revision.Reason,
                                        SubmittedDate = revision.SubmittedDate,
                                        ReceivedDate = revision.ReceivedDate,
                                        ManHours = revision.ManHours,
                                        RevisionBy = revision.RevisionBy,
                                        ExecutedBy = revision.ExecutedBy,
                                        Remark = revision.Remark
                                    };


                if (Id > 0)
                {
                    revisionQuery = revisionQuery.Where(o => o.ProjectId == Id); 
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    revisionQuery = revisionQuery.Where(o => o.ScopeOfWork.Contains(searchTerm) ||
                                             o.Reason.Contains(searchTerm) ||
                                             o.ManHours.Contains(searchTerm) ||
                                             o.RevisionBy.Contains(searchTerm) ||
                                             o.ExecutedBy.Contains(searchTerm) ||
                                             o.Remark.Contains(searchTerm));
                }

                int totalCount = await revisionQuery.CountAsync();

                var revisionList = await revisionQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (revisionList, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (Enumerable.Empty<RevisionLogsViewModel>(), 0);
            }
        }

        //public async Task AddRevisionLogsAsync(RevisionLogsViewModel revisionLogs)
        //{
        //    var addRevisionLogs = new TblRevision
        //    {
        //        ProjectId = revisionLogs.ProjectId,
        //        ScopeOfWork = revisionLogs.ScopeOfWork,
        //        Reason = revisionLogs.Reason,
        //        SubmittedDate = revisionLogs.SubmittedDate,
        //        ReceivedDate = revisionLogs.ReceivedDate,
        //        ManHours = revisionLogs.ManHours,
        //        RevisionBy = revisionLogs.RevisionBy,
        //        ExecutedBy = revisionLogs.ExecutedBy,
        //        Remark = revisionLogs.Remark,
        //        CreatedAt = DateTime.Now,
        //        CreatedBy = 2,
        //        IsDeleted = false
        //    };
        //    _context.TblRevisions.Add(addRevisionLogs);
        //    await _context.SaveChangesAsync();
        //}


        public async Task AddRevisionLogsAsync(RevisionLogsViewModel revisionLogs)
        {
            string newSubId;

            var existingSubIds = await _context.TblRevisions
                .Where(x => x.ProjectId == revisionLogs.ProjectId)
                .Select(x => x.SubId)
                .ToListAsync();

            if (existingSubIds.Any())
            {

                var maxSubId = existingSubIds
                    .Select(subId => decimal.Parse(subId))
                    .Max();

                newSubId = $"{(maxSubId + 0.1M):0.0}";
            }
            else
            {
                newSubId = "1.1";
            }

            var addRevisionLogs = new TblRevision
            {
                ProjectId = revisionLogs.ProjectId,
                SubId = newSubId, 
                ScopeOfWork = revisionLogs.ScopeOfWork,
                Reason = revisionLogs.Reason,
                SubmittedDate = revisionLogs.SubmittedDate,
                ReceivedDate = revisionLogs.ReceivedDate,
                ManHours = revisionLogs.ManHours,
                RevisionBy = revisionLogs.RevisionBy,
                ExecutedBy = revisionLogs.ExecutedBy,
                Remark = revisionLogs.Remark,
                CreatedAt = DateTime.Now,
                CreatedBy = revisionLogs.CreatedBy,
                IsDeleted = false
            };

            if (revisionLogs.DocumentName != null && revisionLogs.DocumentName.Any())
            {
                var uploadsFolderPath = Path.Combine("wwwroot", "revisionDocuments");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var uploadedFilePaths = new List<string>();

                foreach (var document in revisionLogs.DocumentName)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(document.FileName);
                    var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }

                    uploadedFilePaths.Add(Path.Combine("revisionDocuments", uniqueFileName));
                }

                addRevisionLogs.Document = string.Join(";", uploadedFilePaths);
            }

            _context.TblRevisions.Add(addRevisionLogs);
            await _context.SaveChangesAsync();
        }

        //public async Task AddRevisionLogsAsync(RevisionLogsViewModel revisionLogs, List<IFormFile> uploadedDocuments)
        //{
        //    string newSubId;

        //    var existingSubIds = await _context.TblRevisions
        //        .Where(x => x.ProjectId == revisionLogs.ProjectId)
        //        .Select(x => x.SubId)
        //        .ToListAsync();

        //    if (existingSubIds.Any())
        //    {
        //        var maxSubId = existingSubIds
        //            .Select(subId => decimal.Parse(subId))
        //            .Max();

        //        newSubId = $"{(maxSubId + 0.1M):0.0}";
        //    }
        //    else
        //    {
        //        newSubId = "1.1";
        //    }

        //    var addRevisionLogs = new TblRevision
        //    {
        //        ProjectId = revisionLogs.ProjectId,
        //        SubId = newSubId,
        //        ScopeOfWork = revisionLogs.ScopeOfWork,
        //        Reason = revisionLogs.Reason,
        //        SubmittedDate = revisionLogs.SubmittedDate,
        //        ReceivedDate = revisionLogs.ReceivedDate,
        //        ManHours = revisionLogs.ManHours,
        //        RevisionBy = revisionLogs.RevisionBy,
        //        ExecutedBy = revisionLogs.ExecutedBy,
        //        Remark = revisionLogs.Remark,
        //        CreatedAt = DateTime.Now,
        //        CreatedBy = 2,
        //        IsDeleted = false
        //    };

        //    _context.TblRevisions.Add(addRevisionLogs);
        //    await _context.SaveChangesAsync();

        //    var revisionId = addRevisionLogs.RevisionId;
        //    // Save attached documents
        //    foreach (var document in uploadedDocuments)
        //    {

        //        var documentName = Path.GetFileName(document.FileName);

        //        var attachDocument = new AttachDocument
        //        {
        //            FkId = revisionId, 
        //            DocumentName = documentName,
        //            Flag = 0,
        //            IsDeleted = false
        //        };

        //        _context.AttachDocuments.Add(attachDocument);

        //        var filePath = Path.Combine("wwwroot/uploads", documentName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await document.CopyToAsync(stream);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

    }
}

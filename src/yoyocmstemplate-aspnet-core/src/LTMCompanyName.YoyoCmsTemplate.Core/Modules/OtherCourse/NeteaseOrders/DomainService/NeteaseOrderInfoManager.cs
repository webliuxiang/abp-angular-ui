using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.OtherCourse.NeteaseOrders.DomainService
{
    public class NeteaseOrderInfoManager : YoyoCmsTemplateDomainServiceBase, INeteaseOrderInfoManager
    {
        private readonly IRepository<NeteaseOrderInfo, long> _repository;
        public NeteaseOrderInfoManager(
            IRepository<NeteaseOrderInfo, long> repository
            )
        {
            _repository = repository;
        }



        public async Task Create(List<NeteaseOrderInfo> entitys, string platform)
        {

            

            var newOrderNos = entitys.Select(o => o.OrderNo).ToList();
           
            // 旧的订单已存在的
            var orderInfos = await _repository.GetAll()
                                            .AsNoTracking()
                                            .Where(o => o.Platform == platform && newOrderNos.Contains(o.OrderNo))
                                            .ToListAsync();

            // 已经存在的数据，要做更新的部分
            var updateOrders = entitys.Where(o => orderInfos.Exists(e => e.OrderNo == o.OrderNo)).ToList();

            // 删除已存在的订单数据
            updateOrders.ForEach(o => entitys.Remove(o));

            // 插入新数据
            foreach (var entity in entitys)
            {
                await _repository.InsertAsync(entity);
            }

            // 更新旧的数据
            foreach (var entity in updateOrders)
            {
                var tmpEntity = orderInfos.Find(o => o.OrderNo == entity.OrderNo);
                if (tmpEntity == null)
                {
                    continue;
                }

                entity.Id = tmpEntity.Id;
                entity.IsChecked = tmpEntity.IsChecked;

                await this.Update(entity);
            }
        }


        public async Task Update(NeteaseOrderInfo entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task UpdateChecked(long id, bool isChecked)
        {
            var entity = await _repository.GetAsync(id);
            entity.IsChecked = isChecked;
            await this.Update(entity);
        }


        public async Task<NeteaseOrderInfo> GetLastOrder()
        {
            return await _repository.GetAll()
                .AsNoTracking()
                .LastOrDefaultAsync();
        }


        public async Task<NeteaseOrderInfo> GetOrderById(long id)
        {
            return await _repository.GetAll()
                .AsNoTracking()
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }


        public async Task UpdateChecked(List<long> entityIds, bool isChecked)
        {
            var entitys = await _repository.GetAll()
                .Where(o => entityIds.Contains(o.Id))
                .ToListAsync();

            foreach (var entity in entitys)
            {
                entity.IsChecked = isChecked;
                await this.Update(entity);
            }
        }


        public async Task UpdateGiteeChecked(long id, bool isChecked)
        {
            var entity = await _repository.GetAsync(id);
            entity.IsGiteeChecked = isChecked;
            await this.Update(entity);
        }

        public async Task UpdateGiteeChecked(List<long> entityIds, bool isChecked)
        {
            var entitys = await _repository.GetAll()
                .Where(o => entityIds.Contains(o.Id))
                .ToListAsync();

            foreach (var entity in entitys)
            {
                entity.IsGiteeChecked = isChecked;
                await this.Update(entity);
            }
        }
    }
}

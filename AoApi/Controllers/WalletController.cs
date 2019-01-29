using AoApi.Data.Models;
using AoApi.Services.Data.DtoModels.WalletDtos;
using AoApi.Services.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoApi.Controllers
{
    [Route("api/employees/{employeeId}/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;

        public WalletController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalletsAsync([FromRoute] Guid employeeId)
        {
            if (!await _walletRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundWallet = await _walletRepository.GetAllByConditionAsync(s => s.EmployeeId == employeeId);

            var walletToReturn = Mapper.Map<IEnumerable<WalletDto>>(foundWallet);

            return Ok(walletToReturn);
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetOneWalletAsync([FromRoute] Guid employeeId, [FromRoute] Guid walletId)
        {
            if (!await _walletRepository.EntityExists<Employee>(e => e.Id == employeeId))
            {
                return NotFound();
            }

            var foundWallet = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletId);

            if (foundWallet == null)
            {
                return NotFound();
            }

            var walletToReturn = Mapper.Map<WalletDto>(foundWallet);

            return Ok(walletToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWalletAsync([FromBody] WalletCreateDto walletToCreate)
        {
            var walletToAdd = Mapper.Map<Wallet>(walletToCreate);
            walletToAdd.Id = Guid.NewGuid();

            _walletRepository.Create(walletToAdd);

            await _walletRepository.SaveChangesAsync();

            var foundWallet = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletToAdd.Id);

            return Ok();
        }

        [HttpPut("{walletId}")]
        public async Task<IActionResult> UpdateWalletAsync([FromRoute] Guid walletId, [FromBody] WalletUpdateDto walletToUpdate)
        {
            var foundWallet = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletId);

            if (foundWallet == null)
            {
                var walletToAdd = Mapper.Map<Wallet>(walletToUpdate);
                walletToAdd.Id = Guid.NewGuid();

                _walletRepository.Create(walletToAdd);

                await _walletRepository.SaveChangesAsync();

                var walletToReturn = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletToAdd.Id);

                return CreatedAtRoute("GetWallet", new { walletId = walletToReturn.Id }, walletToReturn);
            }

            Mapper.Map(walletToUpdate, foundWallet);

            _walletRepository.Update(foundWallet);

            await _walletRepository.SaveChangesAsync();

            // check if it returns "old" wallet before update. Else find and return?
            return Ok(foundWallet);

            //return NoContent();
        }

        [HttpPatch("{walletId}")]
        public async Task<IActionResult> PartialUpdateWalletAsync([FromRoute] Guid walletId, [FromBody] JsonPatchDocument<WalletUpdateDto> walletToPartialUpdate)
        {
            if (walletToPartialUpdate == null)
                return BadRequest();

            var foundWallet = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletId);

            if (foundWallet == null)
            {
                var walletToCreate = new WalletUpdateDto();

                // check if necessary
                walletToPartialUpdate.ApplyTo(walletToCreate, ModelState);

                var walletToAdd = Mapper.Map<Wallet>(walletToCreate);
                walletToAdd.Id = Guid.NewGuid();

                _walletRepository.Create(walletToAdd);

                await _walletRepository.SaveChangesAsync();

                var walletToReturn = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletToAdd.Id);

                return CreatedAtRoute("GetWallet", new { walletId = walletToReturn.Id }, walletToReturn);
            }
            // why map back and fourth?
            var walletToPatch = Mapper.Map<WalletUpdateDto>(foundWallet);

            // check if necessary
            walletToPartialUpdate.ApplyTo(walletToPatch, ModelState);

            // why map back and fourth?
            Mapper.Map(walletToPatch, foundWallet);

            _walletRepository.Update(foundWallet);

            await _walletRepository.SaveChangesAsync();

            // check if it returns "old" wallet before update. Else find and return?
            return Ok(foundWallet);

            //return NoContent();
        }

        [HttpDelete("{walletId}")]
        public async Task<IActionResult> DeleteWalletAsync([FromRoute] Guid walletId)
        {
            var foundWallet = await _walletRepository.GetFirstByConditionAsync(j => j.Id == walletId);

            if (foundWallet == null)
                return NotFound();

            _walletRepository.Delete(foundWallet);

            await _walletRepository.SaveChangesAsync();

            return Ok();
        }
    }
}

import { CashRegisterResponse } from '@api/generated/models';

export const filterCashRegisters = (
  cashRegisters: CashRegisterResponse[],
  filters: { searchName?: string }
) => {
  return cashRegisters.filter((cashRegister) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName ||
      cashRegister.name!.toLowerCase().includes(filters.searchName.toLowerCase());
    return nameSearchFilter;
  });
};

import { CustomerResponse } from '@api/generated/models';

export const filterCustomers = (
  customers: CustomerResponse[],
  filters: { searchName?: string; countryStateIds: number[] }
) => {
  return customers.filter((customer) => {
    // name filter
    const fullname = `${customer.name} ${customer.surname} ${customer.secondSurname}`;
    const nameSearchFilter =
      !filters.searchName || fullname.toLowerCase().includes(filters.searchName.toLowerCase());

    // state filter
    const stateFilter =
      !filters.countryStateIds ||
      filters.countryStateIds.length === 0 ||
      filters.countryStateIds.includes(customer.countryState?.id ?? 0);

    return nameSearchFilter && stateFilter;
  });
};

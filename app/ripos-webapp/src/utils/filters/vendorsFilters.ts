import { VendorResponse } from '@api/generated/models';

export const filterVendors = (
  vendors: VendorResponse[],
  filters: { searchName?: string; countryStateIds: number[] }
) => {
  return vendors.filter((vendor) => {
    // name filter
    const fullname = `${vendor.name} ${vendor.surname} ${vendor.secondSurname}`;
    const nameSearchFilter =
      !filters.searchName || fullname.toLowerCase().includes(filters.searchName.toLowerCase());

    // state filter
    const stateFilter =
      !filters.countryStateIds ||
      filters.countryStateIds.length === 0 ||
      filters.countryStateIds.includes(vendor.countryState?.id ?? 0);

    return nameSearchFilter && stateFilter;
  });
};

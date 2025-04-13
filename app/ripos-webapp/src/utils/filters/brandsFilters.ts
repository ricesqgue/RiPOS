import { BrandResponse } from '@api/generated/models';

export const filterBrands = (brands: BrandResponse[], filters: { searchName?: string }) => {
  return brands.filter((brand) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName || brand.name!.toLowerCase().includes(filters.searchName.toLowerCase());

    return nameSearchFilter;
  });
};

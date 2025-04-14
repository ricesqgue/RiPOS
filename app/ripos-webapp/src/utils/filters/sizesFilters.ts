import { SizeResponse } from '@api/generated/models';

export const filterSizes = (sizes: SizeResponse[], filters: { searchName?: string }) => {
  return sizes.filter((size) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName ||
      size.name!.toLowerCase().includes(filters.searchName.toLowerCase()) ||
      size.shortName!.toLowerCase().includes(filters.searchName.toLowerCase());

    return nameSearchFilter;
  });
};

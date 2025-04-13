import { GenderResponse } from '@api/generated/models';

export const filterGenders = (genders: GenderResponse[], filters: { searchName?: string }) => {
  return genders.filter((gender) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName || gender.name!.toLowerCase().includes(filters.searchName.toLowerCase());

    return nameSearchFilter;
  });
};

import { CategoryResponse } from '@api/generated/models';

export const filterCategories = (
  categories: CategoryResponse[],
  filters: { searchName?: string }
) => {
  return categories.filter((category) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName ||
      category.name!.toLowerCase().includes(filters.searchName.toLowerCase());
    return nameSearchFilter;
  });
};

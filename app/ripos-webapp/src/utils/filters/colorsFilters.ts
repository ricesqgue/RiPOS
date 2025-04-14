import { ColorResponse } from '@api/generated/models';

export const filterColors = (colors: ColorResponse[], filters: { searchName?: string }) => {
  return colors.filter((color) => {
    // name filter
    const nameSearchFilter =
      !filters.searchName ||
      color.name!.toLowerCase().includes(filters.searchName.toLowerCase()) ||
      color.rgbHex!.toLowerCase().includes(filters.searchName.toLowerCase());

    return nameSearchFilter;
  });
};

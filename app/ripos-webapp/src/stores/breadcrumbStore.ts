import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { create } from 'zustand';

interface BreadcrumbState {
  breadcrumbItems: ItemType[];
  setBreadcrumbItems: (breadcrumbItems: ItemType[]) => void;
  addBreadcrumbItem: (breadcrumbItem: ItemType) => void;
}

const useBreadcrumbStore = create<BreadcrumbState>((set) => ({
  breadcrumbItems: [],
  setBreadcrumbItems: (items) => set({ breadcrumbItems: items }),
  addBreadcrumbItem: (item) =>
    set((state) => ({ breadcrumbItems: [...state.breadcrumbItems, item] })),
}));

export { useBreadcrumbStore };

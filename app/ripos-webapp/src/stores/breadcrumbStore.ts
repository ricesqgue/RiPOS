import { ItemType } from 'antd/es/breadcrumb/Breadcrumb';
import { create } from 'zustand';

interface BreadcrumbState {
  breadcrumbItems: ItemType[];
  setBreadcrumbItems: (breadcrumbItems: ItemType[]) => void;
}

const useBreadcrumbStore = create<BreadcrumbState>((set) => ({
  breadcrumbItems: [],
  setBreadcrumbItems: (items) => set({ breadcrumbItems: items }),
}));

export { useBreadcrumbStore };

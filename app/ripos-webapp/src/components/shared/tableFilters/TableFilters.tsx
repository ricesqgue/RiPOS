import { Flex } from 'antd';
import { ReactNode } from 'react';
import styles from './tableFilters.module.scss';

export type TebleFiltersType = { filterGroupTitle?: string; components: ReactNode[] };

interface TableFiltersProps {
  filters: TebleFiltersType[];
}

const TableFilters = (props: TableFiltersProps) => {
  return (
    <Flex gap={8} vertical>
      {props.filters.map((filterGroup, i) => (
        <div key={`filter-group-container-${i}`}>
          {filterGroup.filterGroupTitle && (
            <span className={styles.filterGroupTitle} key={`filter-group-title-${i}`}>
              {filterGroup.filterGroupTitle}
            </span>
          )}
          <Flex gap={8} wrap="wrap" key={`filter-group-${i}`}>
            {filterGroup.components}
          </Flex>
        </div>
      ))}
    </Flex>
  );
};

export default TableFilters;

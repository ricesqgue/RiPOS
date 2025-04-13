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
        <>
          {filterGroup.filterGroupTitle && (
            <span className={styles.filterGroupTitle}>{filterGroup.filterGroupTitle}</span>
          )}
          <Flex gap={8} wrap="wrap" key={`filter-group-${i}`}>
            {filterGroup.components}
          </Flex>
        </>
      ))}
    </Flex>
  );
};

export default TableFilters;

import { Button, Flex, Switch } from 'antd';
import type { TebleFiltersType } from '../tableFilters/TableFilters';
import TableFilters from '../tableFilters/TableFilters';
import styles from './tableToolbar.module.scss';

interface ToolbarButton {
  text: string;
  onClick: () => void;
}

interface TableFilterToolbarProps {
  filters?: TebleFiltersType[];
  options?: {
    includeInactivesSwitch?: {
      label?: string;
      value: boolean;
      onChange: (value: boolean) => void;
    };
    buttons?: ToolbarButton[];
  };
}

const TableToolbar = (props: TableFilterToolbarProps) => {
  return (
    <Flex vertical gap={8}>
      {props.filters && <TableFilters filters={props.filters}></TableFilters>}

      {props.options && (
        <Flex
          id="toolbarOptions"
          wrap="wrap"
          justify="space-between"
          align="center"
          gap={5}
          className={styles.toolbarOptions}
        >
          <Flex id="leftOptions">
            {props.options.includeInactivesSwitch && (
              <Flex align="center" gap={5}>
                <span className={styles.swichLabel}>Mostar inactivos</span>{' '}
                <Switch
                  size="small"
                  value={props.options.includeInactivesSwitch.value}
                  onChange={props.options.includeInactivesSwitch.onChange}
                ></Switch>
              </Flex>
            )}
          </Flex>
          <Flex id="rightOptions" align="center" gap={5} wrap="wrap">
            {props.options.buttons &&
              props.options.buttons.map((button, i) => (
                <Button
                  onClick={button.onClick}
                  size="small"
                  variant="filled"
                  color="primary"
                  className={styles.toolbarButton}
                  key={`table-button-${i}`}
                >
                  {button.text}
                </Button>
              ))}
          </Flex>
        </Flex>
      )}
    </Flex>
  );
};

export default TableToolbar;

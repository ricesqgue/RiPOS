import styles from './PageContent.module.scss';

interface PageContentProps {
  children: React.ReactNode;
}
const PageContent = (props: PageContentProps) => {
  return (
    <div className={styles.pageContainer}>
      <div className={styles.pageContent}>{props.children}</div>
      <div className={styles.pageFooter}>footer</div>
    </div>
  );
};

export default PageContent;

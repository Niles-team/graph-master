import { Theme } from '@material-ui/core/styles/createMuiTheme';

export function mergeStyles(...styles: any[]) {
  return function(theme: Theme) {
    const evaluatedStyles = styles.map((style) => {
      if (typeof style === 'function') {
        return style(theme);
      } else {
        return style;
      }
    })

    return evaluatedStyles.reduce((acc, val) => Object.assign(acc, val), []);
  }
};
// import { History } from "history";

// const noInit = {}
// const storageKey = 'app_voyage';
// const searchParamName = 'impersonatedTenantId';
// class SessionService {
//     hookHistory(history: History) {
//         const originalPush = history.push;
//         const originalReplace = history.replace;
//         history.push = async (a: string | LocationDescriptorObject, b?) => {
//             const storageItem = await this.getStorageItem();
//             if (typeof a === 'string') {
//                 if (storageItem.impersonatedTenantId) {
//                     a = a.includes('?') ?
//                         a + `&${searchParamName}=${storageItem.impersonatedTenantId}` :
//                         a + `?${searchParamName}=${storageItem.impersonatedTenantId}`;
//                 }
//                 originalPush(a, b)
//             } else {
//                 if (storageItem.impersonatedTenantId) {
//                     if (a.search) {
//                         a = { ...a, search: a.search + `&${searchParamName}=${storageItem.impersonatedTenantId}` }
//                     } else {
//                         a = { ...a, search: `?${searchParamName}=${storageItem.impersonatedTenantId}` }
//                     }
//                 }
//                 originalPush(a)
//             }
//         };
//         history.replace = async (a: string | LocationDescriptorObject, b?) => {
//             const storageItem = await this.getStorageItem();
//             if (typeof a === 'string') {
//                 if (storageItem.impersonatedTenantId) {
//                     a = a.includes('?') ?
//                         a + `&${searchParamName}=${storageItem.impersonatedTenantId}` :
//                         a + `?${searchParamName}=${storageItem.impersonatedTenantId}`;
//                 }
//                 originalReplace(a, b)
//             } else {
//                 if (storageItem.impersonatedTenantId) {
//                     if (a.search) {
//                         a = { ...a, search: a.search + `&${searchParamName}=${storageItem.impersonatedTenantId}` }
//                     } else {
//                         a = { ...a, search: `?${searchParamName}=${storageItem.impersonatedTenantId}` }
//                     }
//                 }
//                 originalReplace(a)
//             }
//         };
//     }
// }
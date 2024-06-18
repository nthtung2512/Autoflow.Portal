import { castObjKeys } from './object-extensions';
import { capitalizeFirstLetter } from './string-extensions';

const Extensions = { capitalizeFirstLetter, castObjKeys } as const;

export default Extensions;

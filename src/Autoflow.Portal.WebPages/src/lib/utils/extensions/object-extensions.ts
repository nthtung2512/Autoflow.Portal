/**
 * @package
 */
const castObjKeys = <T extends string | number | symbol>(obj: { [id in T]: any }): T[] => {
	return Object.keys(obj) as T[];
};

export { castObjKeys };

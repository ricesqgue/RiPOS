/**
 * Generated by orval v7.7.0 🍺
 * Do not edit manually.
 * RiPOS API
 * OpenAPI spec version: v1
 */
import { useQuery } from '@tanstack/react-query';
import type {
  DataTag,
  DefinedInitialDataOptions,
  DefinedUseQueryResult,
  QueryFunction,
  QueryKey,
  UndefinedInitialDataOptions,
  UseQueryOptions,
  UseQueryResult,
} from '@tanstack/react-query';

import type { CountryStateResponse } from '.././models';

import getApiMiscCountryStatesMutator from '../../axiosMutator';

export const getApiMiscCountryStates = (signal?: AbortSignal) => {
  return getApiMiscCountryStatesMutator<CountryStateResponse[]>({
    url: `/api/misc/countryStates`,
    method: 'GET',
    signal,
  });
};

export const getGetApiMiscCountryStatesQueryKey = () => {
  return [`/api/misc/countryStates`] as const;
};

export const getGetApiMiscCountryStatesQueryOptions = <
  TData = Awaited<ReturnType<typeof getApiMiscCountryStates>>,
  TError = void,
>(options?: {
  query?: Partial<
    UseQueryOptions<Awaited<ReturnType<typeof getApiMiscCountryStates>>, TError, TData>
  >;
}) => {
  const { query: queryOptions } = options ?? {};

  const queryKey = queryOptions?.queryKey ?? getGetApiMiscCountryStatesQueryKey();

  const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiMiscCountryStates>>> = ({
    signal,
  }) => getApiMiscCountryStates(signal);

  return { queryKey, queryFn, ...queryOptions } as UseQueryOptions<
    Awaited<ReturnType<typeof getApiMiscCountryStates>>,
    TError,
    TData
  > & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type GetApiMiscCountryStatesQueryResult = NonNullable<
  Awaited<ReturnType<typeof getApiMiscCountryStates>>
>;
export type GetApiMiscCountryStatesQueryError = void;

export function useGetApiMiscCountryStates<
  TData = Awaited<ReturnType<typeof getApiMiscCountryStates>>,
  TError = void,
>(options: {
  query: Partial<
    UseQueryOptions<Awaited<ReturnType<typeof getApiMiscCountryStates>>, TError, TData>
  > &
    Pick<
      DefinedInitialDataOptions<
        Awaited<ReturnType<typeof getApiMiscCountryStates>>,
        TError,
        Awaited<ReturnType<typeof getApiMiscCountryStates>>
      >,
      'initialData'
    >;
}): DefinedUseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiMiscCountryStates<
  TData = Awaited<ReturnType<typeof getApiMiscCountryStates>>,
  TError = void,
>(options?: {
  query?: Partial<
    UseQueryOptions<Awaited<ReturnType<typeof getApiMiscCountryStates>>, TError, TData>
  > &
    Pick<
      UndefinedInitialDataOptions<
        Awaited<ReturnType<typeof getApiMiscCountryStates>>,
        TError,
        Awaited<ReturnType<typeof getApiMiscCountryStates>>
      >,
      'initialData'
    >;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };
export function useGetApiMiscCountryStates<
  TData = Awaited<ReturnType<typeof getApiMiscCountryStates>>,
  TError = void,
>(options?: {
  query?: Partial<
    UseQueryOptions<Awaited<ReturnType<typeof getApiMiscCountryStates>>, TError, TData>
  >;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> };

export function useGetApiMiscCountryStates<
  TData = Awaited<ReturnType<typeof getApiMiscCountryStates>>,
  TError = void,
>(options?: {
  query?: Partial<
    UseQueryOptions<Awaited<ReturnType<typeof getApiMiscCountryStates>>, TError, TData>
  >;
}): UseQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
  const queryOptions = getGetApiMiscCountryStatesQueryOptions(options);

  const query = useQuery(queryOptions) as UseQueryResult<TData, TError> & {
    queryKey: DataTag<QueryKey, TData, TError>;
  };

  query.queryKey = queryOptions.queryKey;

  return query;
}

/**
 * Generated by orval v7.7.0 🍺
 * Do not edit manually.
 * RiPOS API
 * OpenAPI spec version: v1
 */

export interface VendorRequest {
  /**
   * @minLength 1
   * @maxLength 50
   */
  name: string;
  /**
   * @minLength 1
   * @maxLength 50
   */
  surname: string;
  /**
   * @maxLength 50
   * @nullable
   */
  secondSurname?: string | null;
  /**
   * @maxLength 20
   * @nullable
   */
  phoneNumber?: string | null;
  /**
   * @maxLength 20
   * @nullable
   */
  mobilePhone?: string | null;
  /**
   * @maxLength 100
   * @nullable
   */
  email?: string | null;
  /**
   * @maxLength 400
   * @nullable
   */
  address?: string | null;
  /**
   * @maxLength 100
   * @nullable
   */
  city?: string | null;
  /**
   * @maxLength 10
   * @nullable
   */
  zipCode?: string | null;
  countryStateId?: number;
}
